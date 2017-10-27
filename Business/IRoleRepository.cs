using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
   public interface IRoleRepository
    {
        IEnumerable<RoleModel> GetAllRoles();
    }
}
