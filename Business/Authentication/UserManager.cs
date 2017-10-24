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

        public long? Authenticate(string token)
        {
            //Forward to authenticator
            return auth.AuthenticateGetId(token);
        }

        public List<IGroup> GetGroupsForUser(long userId)
        {
            return users.GetGroupsForUser(userId);
        }
    }
}
