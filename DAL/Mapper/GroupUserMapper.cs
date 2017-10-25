using Business.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Mapper
{
    class GroupUserMapper
    {
        public static GroupUser MapGroupUserModelToEntity(GroupUserModel groupUserModel)
        {
            if (groupUserModel == null) return null;
            return new GroupUser()
            {
                GroupId = groupUserModel.Group.Id == null ? 0 : (int)groupUserModel.Group.Id,
                UserId = groupUserModel.User.Id == null ? 0 : (int)groupUserModel.User.Id,
                IsAdministrator = groupUserModel.IsAdministrator
            };
        }

        public static GroupUserModel MapGroupUserEntityToModel(GroupUser groupUser)
        {
            if (groupUser == null) return null;
            return new GroupUserModel()
            {
                Group = GroupMapper.MapGroupEntityToModel(groupUser.Group),
                User = UserMapper.MapUserEntityToModel(groupUser.User),
                IsAdministrator = groupUser.IsAdministrator == null ? false : (bool)groupUser.IsAdministrator
            };
        }
    }
}
