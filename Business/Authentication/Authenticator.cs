using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

[assembly: InternalsVisibleTo("ChaTexTest")]
namespace Business.Authentication
{
    class Authenticator : IAuthenticator
    {
        private readonly IUserRepository userRepository;
        private readonly ReaderWriterLock userLock;

        public Authenticator(IUserRepository userRepository)
        {
            this.userRepository = userRepository;

            userLock = new ReaderWriterLock();
        }

        /// <summary>
        /// Logs in the user with the specified email and password and generates a token for future authentication. The token will
        /// expire after 1 day. This method is thread safe.
        /// </summary>
        /// <param name="email">The email of the user to log in</param>
        /// <param name="password">The password of the user to log in</param>
        /// <returns>The generated token, or null if the user could not be authorized</returns>
        public string Login(string email, string password)
        {
            if (!areCredentialsValid(email, password)) return null;

            int userId = (int)userRepository.GetUserIdFromEmail(email);

            try
            {
                //Attempt to get existing token
                //This must be protected against other threads, as we need to ensure that other threads do not acquire an outdated token while this thread updates it
                //Ideally, we would lock only on the specific user id, but it was uncertain if such a locking mechanism exists
                userLock.AcquireWriterLock(Timeout.Infinite);
                return loginInternal(userId);
            }
            finally
            {
                userLock.ReleaseLock();
            }
        }

        /// <summary>
        /// Get the id of the user with the specified token, if such a token exists. This method is entirely thread safe.
        /// </summary>
        /// <param name="token">The user's token</param>
        /// <returns>The id of the user with the specified token, or null if the token is not recognized</returns>
        public int? GetUserIdFromToken(string token)
        {
            //The following check does not need to be synchronized, as the token will not be updated if it is not expired
            if (!isTokenExpired(token))
            {
                return userRepository.GetUserIdFromToken(token);
            }

            return null;
        }

        private string loginInternal(int userId)
        {
            //This implementation is not thread safe, but it is expected that the caller handles synchronization
            string token = userRepository.GetSessionToken(userId);

            //If the token is expired, it must be removed from the database
            if (token != null && isTokenExpired(token))
            {
                userRepository.DeleteUserToken(userId);
                token = null;
            }

            //If there is no existing token at this point, a new one is generated
            if (token == null)
            {
                token = generateToken(DateTime.Now.ToUniversalTime().AddDays(1));
                userRepository.SaveUserToken(userId, token);
            }

            return token;
        }

        private bool areCredentialsValid(string email, string password)
        {
            //No need to synchronize this as user emails, ids, passwords etc. are never changed
            int? userId = userRepository.GetUserIdFromEmail(email);

            if (userId == null) return false;

            byte[] salt = userRepository.GetUserSalt((int)userId);
            string correctHash = userRepository.GetUserPasswordHash((int)userId);

            //Generate hash based on the provided password and the retrieved salt
            string passwordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512, 65536, 32));
            return passwordHash.Equals(correctHash);
        }

        /// <summary>
        /// Generate a new token with the specified expiration date. Note that this token is not secure, and is only used for illustrational purposes.
        /// </summary>
        /// <param name="expiration">The expiraiton date of the token</param>
        /// <returns>The generated token in base 64</returns>
        private string generateToken(DateTime expiration)
        {
            //From:
            //https://stackoverflow.com/questions/14643735/how-to-generate-a-unique-token-which-expires-after-24-hours
            byte[] guid = Guid.NewGuid().ToByteArray();
            byte[] exp = BitConverter.GetBytes(expiration.ToBinary());
            return Convert.ToBase64String(exp.Concat(guid).ToArray());
        }

        /// <summary>
        /// Check if the expiration date encoded in the specified token has been passed.
        /// </summary>
        /// <param name="token">The token to check</param>
        /// <returns>True if the token is expired, false otherwise</returns>
        private Boolean isTokenExpired(string token)
        {
            try
            {
                //From:
                //https://stackoverflow.com/questions/14643735/how-to-generate-a-unique-token-which-expires-after-24-hours
                byte[] data = Convert.FromBase64String(token);
                DateTime expiration = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
                return DateTime.Now > expiration;
            }
            catch (Exception e) when (e is FormatException || e is ArgumentException)
            {
                //Invalid token
                return true;
            }
        }
    }
}
