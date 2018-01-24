using Business.Models;
using System.Collections.Generic;

namespace Business.Users
{
    public interface IUserManager
    {
        string Login(string email, string password);

        IEnumerable<UserModel> GetAllUsers();

        void UpdateUser(int callerId, UserModel userModel);

        IEnumerable<RoleModel> GetAllUserRoles(int userId);
    }
}
