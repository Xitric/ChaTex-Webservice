using Business.Models;
using DAL.Models;

namespace DAL.Mapper
{
    class GroupRoleMapper
    {
        public static GroupRole MapGroupRoleModelToEntity(GroupRoleModel groupRoleModel)
        {
            if (groupRoleModel == null)
            {
                return null;
            }

            return new GroupRole()
            {
                GroupId = groupRoleModel.Group.Id == null ? 0 : groupRoleModel.Group.Id.Value,
                RoleId = groupRoleModel.Role.Id == null ? 0 : groupRoleModel.Role.Id.Value
            };
        }

        public static GroupRoleModel MapGroupRoleEntityToModel(GroupRole groupRole)
        {
            if (groupRole == null)
            {
                return null;
            }

            return new GroupRoleModel()
            {
                Group = GroupMapper.MapGroupEntityToModel(groupRole.Group),
                Role = RoleMapper.MapRoleEntityToModel(groupRole.Role)
            };
        }
    }
}
