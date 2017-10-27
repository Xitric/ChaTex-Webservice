using Business.Models;
using System.Collections.Generic;

namespace Business.Users
{
    public interface IUserManager
    {
        string Login(string email);

        IEnumerable<UserModel> GetAllUsers();

        IEnumerable<GroupModel> GetGroupsForUser(int userId);


    }
}
