﻿using Business.Models;
using System.Collections.Generic;

namespace Business.Authentication
{
    public interface IUserManager
    {
        string Login(string email);

        List<IGroup> GetGroupsForUser(long userId, string token);
    }
}
