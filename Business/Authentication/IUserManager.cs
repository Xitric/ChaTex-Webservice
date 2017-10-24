using Business.Models;
using System.Collections.Generic;

namespace Business.Authentication
{
    public interface IUserManager
    {
        string Login(string email);

        List<GroupModel> GetGroupsForUser(int userId);
    }
}
