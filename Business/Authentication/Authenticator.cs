using Business;
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
        /// <returns>The generated token, or null if the user could not be authorized</returns>
        public string Login(string email)
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

                if (users.SaveUserToken(email, token))
                {
                    return token;
                }

                //For some reason the token could not be generated, and the login failed
                return null;
            }

            return token;
        }

        /// <summary>
        /// Get the id of the user with the specified token. If this method returns null, it means that the user could not be authenticated.
        /// </summary>
        /// <param name="token">The token to check</param>
        /// <returns>The id of the user owning the token, or null if the token is invalid or expired.</returns>
        public long? AuthenticateGetId(string token)
        {
            if (!IsTokenExpired(token))
            {
                return users.GetUserIdFromToken(token);
            }

            return null;
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
