using Business.Models;
using System;
using System.Linq;

namespace Business.Authentication
{
    class Authenticator
    {
        private readonly IUserRepository users;

        public Authenticator(IUserRepository users)
        {
            this.users = users;
        }

        /// <summary>
        /// Logs in the user with the specified email and generates a token for future authentication. The token will
        /// expire after 1 hour.
        /// </summary>
        /// <param name="email">The email of the user to log in</param>
        /// <returns>A user object containing the generated token and user id</returns>
        public User Login(string email)
        {
            //Attempt to get existing token
            string token = users.GetSessionToken(email);

            //If the token is expired, it must be removed from the database
            if (token != null && IsTokenExpired(token))
            {
                users.DeleteUserToken(email);
                token = null;
            }

            //If there is no existing token at this point, a new one is generated
            if (token == null)
            {
                token = GenerateToken(DateTime.Now.AddHours(1));

                if (!users.SaveUserToken(email, token))
                {
                    //For some reason the token could not be generated, and the login failed
                    return null;
                }
            }

            //Login success
            long? userId = users.GetUserIdFromToken(token);

            if (userId == null)
            {
                //Not sure why this should ever happen
                return null;
            }

            return new User(userId, null, null, null, null, token);
        }

        /// <summary>
        /// Authenticate the user based on whether the token is valid and is linked to the specified user ID.
        /// </summary>
        /// <param name="token">The token to check</param>
        /// <param name="userId">The ID of the user</param>
        /// <returns>True if the authentication succeeds, false otherwise</returns>
        public bool Authenticate(string token, long userId)
        {
            if (!IsTokenExpired(token))
            {
                return users.GetUserIdFromToken(token) == userId;
            }

            return false;
        }

        /// <summary>
        /// Generate a new token with the specified expiration date. Note that this token is not secure, and is only used for illustrational purposes.
        /// </summary>
        /// <param name="expiration">The expiraiton date of the token</param>
        /// <returns>The generated token in base 64</returns>
        private string GenerateToken(DateTime expiration)
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
        private Boolean IsTokenExpired(string token)
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
