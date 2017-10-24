using Models.Models;
using System.Collections.Generic;

namespace Business.Authentication
{
    public interface IUserManager
    {
        string Login(string email);

        long? Authenticate(string token);

        List<IGroup> GetGroupsForUser(long userId);
    }
}
