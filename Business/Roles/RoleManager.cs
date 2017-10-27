using System;
using System.Collections.Generic;
using System.Text;
using Business.Models;

namespace Business.Roles
{
    class RoleManager : IRoleManager
    {
        private readonly IRoleRepository roles;

        public RoleManager(IRoleRepository roles)
        {
            this.roles = roles;
        }
        public IEnumerable<RoleModel> GetAllRoles()
        {
            return roles.GetAllRoles();
        }
    }
}
