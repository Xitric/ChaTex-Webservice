using Business.Models;
using System.Collections.Generic;

namespace Business.Authentication
{
    class UserManager : IUserManager
    {
        private readonly IUserRepository users;
        private readonly Authenticator auth;

        public UserManager(IUserRepository users, Authenticator auth)
        {
            this.users = users;
            this.auth = auth;
        }

        public string Login(string email)
        {
            //Forward to authenticator
            return auth.Login(email);
        }

        public List<IGroup> GetGroupsForUser(long userId, string token)
        {
            if (auth.Authenticate(token, userId))
            {
                return users.GetGroupsForUser(userId);
            }

            //Not good, I know, but it will do for now
            throw new AuthException("Authentication failed");
        }
    }
}
