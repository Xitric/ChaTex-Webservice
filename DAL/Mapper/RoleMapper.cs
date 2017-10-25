using Business.Models;
using DAL.Models;

namespace DAL.Mapper
{
    class RoleMapper
    {
        public static Role MapRoleModelToEntity(RoleModel roleModel)
        {
            if (roleModel == null) return null;
            return new Role()
            {
                RoleId = roleModel.Id == null ? 0 : roleModel.Id.Value,
                RoleName = roleModel.Name,
                IsDeleted = roleModel.IsDeleted
            };
        }

        public static RoleModel MapRoleEntityToModel(Role role)
        {
            if (role == null) return null;
            return new RoleModel()
            {
                Id = role.RoleId,
                Name = role.RoleName,
                IsDeleted = role.IsDeleted == null ? false : role.IsDeleted.Value
            };
        }
    }
}
