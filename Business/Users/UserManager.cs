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

        public IEnumerable<UserModel> GetAllUsers()
        {
            return users.GetAllUsers();
        }
        
        public void UpdateUser(int callerId, UserModel userModel)
        {
            if (users.IsUserAdmin(callerId))
            {
                UserModel oldUser = users.GetUser((int)userModel.Id);

                if(userModel.Email != null)
                    oldUser.Email = userModel.Email;

                if (userModel.FirstName != null)
                    oldUser.FirstName = userModel.FirstName;

                if (userModel.MiddleInitial != null)
                    oldUser.MiddleInitial = userModel.MiddleInitial;

                if (userModel.LastName != null)
                    oldUser.LastName = userModel.LastName;
                
                if(userModel.IsDeleted != null)
                    oldUser.IsDeleted = userModel.IsDeleted;

                users.UpdateUser(oldUser);
            }
        }
    }
}
