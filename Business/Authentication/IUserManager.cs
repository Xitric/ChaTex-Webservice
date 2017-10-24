using Business.Models;
using System.Collections.Generic;

namespace Business.Authentication
{
    public interface IUserManager
    {
        string Login(string email);

        long? Authenticate(string token);

        List<GroupModel> GetGroupsForUser(long userId);
    }
}
