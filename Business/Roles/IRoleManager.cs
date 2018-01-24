using Business.Models;
using System.Collections.Generic;

namespace Business.Roles
{
    public interface IRoleManager
    {
        IEnumerable<RoleModel> GetAllRoles();
    }
}
