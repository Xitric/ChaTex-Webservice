using Business;
using System.Collections.Generic;
using Business.Models;
using DAL.Models;
using System.Linq;
using DAL.Mapper;

namespace DAL
{
    class RoleRepository : IRoleRepository
    {
        public IEnumerable<RoleModel> GetAllRoles()
        {
            using (var context = new ChatexdbContext())
            {
                return context.Role.Select(r => RoleMapper.MapRoleEntityToModel(r)).ToList();
            }
        }
    }
}
