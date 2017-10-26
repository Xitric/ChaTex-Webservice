using Business;
using Business.Authentication;
using Business.Models;
using System.Collections.Generic;

namespace Business.Users
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

        public IEnumerable<GroupModel> GetGroupsForUser(int userId)
        {
            return users.GetGroupsForUser(userId);
        }
        
    }
}
