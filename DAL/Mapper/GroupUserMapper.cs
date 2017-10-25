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
            return new GroupUser()
            {
                GroupId = groupUserModel.Group.Id == null ? 0 : (int)groupUserModel.Group.Id,
                UserId = groupUserModel.User.Id == null ? 0 : (int)groupUserModel.User.Id
            };
        }

        public static GroupUserModel MapGroupUserEntityToModel(GroupUser groupUser)
        {
            return new GroupUserModel()
            {
                Group = GroupMapper.MapGroupEntityToModel(groupUser.Group),
                User = UserMapper.MapUserEntityToModel(groupUser.User)
            };
        }
    }
}
