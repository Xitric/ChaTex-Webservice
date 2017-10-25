using Business.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Mapper
{
    class GroupRoleMapper
    {
        public static GroupRole MapGroupRoleModelToEntity(GroupRoleModel groupRoleModel)
        {
            return new GroupRole()
            {
                GroupId = groupRoleModel.Group.Id == null ? 0 : groupRoleModel.Group.Id.Value,
                RoleId = groupRoleModel.Role.Id == null ? 0 : groupRoleModel.Role.Id.Value
            };
        }

        public static GroupRoleModel MapGroupRoleEntityToModel(GroupRole groupRole)
        {
            return new GroupRoleModel()
            {
                Group = GroupMapper.MapGroupEntityToModel(groupRole.Group),
                Role = RoleMapper.MapRoleEntityToModel(groupRole.Role)
            };
        }
    }
}
