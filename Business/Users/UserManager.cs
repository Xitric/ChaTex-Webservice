using Business.Authentication;
using Business.Errors;
using Business.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

[assembly: InternalsVisibleTo("ChaTexTest")]
namespace Business.Users
{

    class UserManager : IUserManager
    {
        private readonly IUserRepository userRepository;
        private readonly Authenticator authenticator;
        private readonly ReaderWriterLock userLock;

        public UserManager(IUserRepository userRepository, Authenticator authenticator)
        {
            this.userRepository = userRepository;
            this.authenticator = authenticator;

            userLock = new ReaderWriterLock();
        }

        public string Login(string email, string password)
        {
            //Forward to authenticator, which is thread safe
            return authenticator.Login(email, password);
        }

        public IEnumerable<UserModel> GetAllUsers()
        {
            return userRepository.GetAllUsers();
        }

        public void UpdateUser(int callerId, UserModel userModel)
        {
            try
            {
                userLock.AcquireWriterLock(Timeout.Infinite);
                updateUserInternal(callerId, userModel);
            }
            finally
            {
                userLock.ReleaseLock();
            }
        }

        /// <summary>
        /// Internal, non-threadsafe method for updating user informaiton. This method expects synchronization to happen elsewhere.
        /// </summary>
        private void updateUserInternal(int callerId, UserModel userModel)
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
                throw new InvalidArgumentException("This operation can only be performed by a system administrator", ParamNameType.CallerId);
            }
        }

        public IEnumerable<RoleModel> GetAllUserRoles(int userId)
        {
            return userRepository.GetAllUserRoles(userId);
        }
    }
}
