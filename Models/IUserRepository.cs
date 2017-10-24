using Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public interface IUserRepository
    {
        string GetSessionToken(string email);

        bool SaveUserToken(string email, string token);

        void DeleteUserToken(string email);

        long? GetUserIdFromToken(string token);

        List<IGroup> GetGroupsForUser(long userId);
    }
}
