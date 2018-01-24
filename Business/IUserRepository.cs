using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public interface IUserRepository
    {
        string GetSessionToken(int userId);

        void SaveUserToken(int userId, string token);

        void DeleteUserToken(int userId);

        int? GetUserIdFromToken(string token);

        int? GetUserIdFromEmail(string email);

        string GetUserPasswordHash(int userId);

        byte[] GetUserSalt(int userId);

        IEnumerable<UserModel> GetAllUsers();

        UserModel GetUser(int userId);
 
        bool IsUserAdmin(int userId);

        void UpdateUser(UserModel userModel);

        IEnumerable<RoleModel> GetAllUserRoles(int userId);
    }
}
