using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Authentication
{
    public interface IAuthenticator
    {
        /// <summary>
        /// Get the id of the user with the specified token. If this method returns null, it means that the user could not be authenticated.
        /// </summary>
        /// <param name="token">The token to check</param>
        /// <returns>The id of the user owning the token, or null if the token is invalid or expired.</returns>
        int? AuthenticateGetId(string token);
    }
}
