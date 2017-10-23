using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Authentication
{
    public interface IUserManager
    {
        string Login(string email);
    }
}
