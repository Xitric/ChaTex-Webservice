using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public interface IUserRepository
    {
        string GetSessionToken(string email);

        bool SaveUserToken(string email, string token);

        void DeleteUserToken(string email);

        int? GetUserIdFromToken(string token);

        List<GroupModel> GetGroupsForUser(int userId);
    }
}
