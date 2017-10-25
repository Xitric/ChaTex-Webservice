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

            };
        }

        public static GroupUserModel MapGroupUserEntityToModel(GroupUser groupUser)
        {
            return new GroupUserModel()
            {

            };
        }
    }
}
