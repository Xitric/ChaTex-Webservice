using Business;
using Business.Authentication;
using Business.Errors;
using Business.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ChaTexTest")]
namespace Business.Users
{
 
    class UserManager : IUserManager
    {
        private readonly IUserRepository userRepository;
        private readonly Authenticator authenticator;

        public UserManager(IUserRepository userRepository, Authenticator authenticator)
        {
            this.userRepository = userRepository;
            this.authenticator = authenticator;
        }

        public string Login(string email)
        {
            //Forward to authenticator
            return authenticator.Login(email);
        }

        public IEnumerable<UserModel> GetAllUsers()
        {
            return userRepository.GetAllUsers();
        }
        
        public void UpdateUser(int callerId, UserModel userModel)
        {
            if (userRepository.IsUserAdmin(callerId))
            {
                UserModel oldUser = userRepository.GetUser((int)userModel.Id);

                if (userModel == null)
                {
                    throw new InvalidArgumentException("User with the specified id does not exist", ParamNameType.UserModel);
                }

                if (userModel.Email != null)
                    oldUser.Email = userModel.Email;

                if (userModel.FirstName != null)
                    oldUser.FirstName = userModel.FirstName;

                if (userModel.MiddleInitial != null)
                    oldUser.MiddleInitial = userModel.MiddleInitial;

                if (userModel.LastName != null)
                    oldUser.LastName = userModel.LastName;
                
                if (userModel.IsDeleted != null)
                    oldUser.IsDeleted = userModel.IsDeleted;

                userRepository.UpdateUser(oldUser);
            }
            else
            {
                throw new InvalidArgumentException("User not system administrator", ParamNameType.CallerId);
            }
        }
    }
}
