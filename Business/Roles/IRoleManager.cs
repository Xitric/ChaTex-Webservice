using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Roles
{
    public interface IRoleManager
    {
        IEnumerable<RoleModel> GetAllRoles();
    }
}
