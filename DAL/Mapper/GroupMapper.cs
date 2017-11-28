using Business.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Mapper
{
    class GroupMapper
    {
        public static Group MapGroupModelToEntity(GroupModel groupModel)
        {
            if (groupModel == null)
            {
                return null;
            }

            return new Group()
            {
                AllowEmployeeAcknowledgeable = groupModel.AllowEmployeeAcknowledgeable,
                AllowEmployeeBookmark = groupModel.AllowEmployeeBookmark,
                AllowEmployeeSticky = groupModel.AllowEmployeeSticky,
                Channel = groupModel.Channels.Select(ch => ChannelMapper.MapChannelModelToEntity(ch)).ToList(),
                CreatedBy = groupModel.Creator.Id == null ? 0 : groupModel.Creator.Id.Value,
                GroupId = groupModel.Id == null ? 0 : groupModel.Id.Value,
                IsDeleted = groupModel.IsDeleted,
                Name = groupModel.Name
            };
        }

        public static GroupModel MapGroupEntityToModel(Group group)
        {
            if (group == null)
            {
                return null;
            }

            return new GroupModel()
            {
                AllowEmployeeAcknowledgeable = group.AllowEmployeeAcknowledgeable,
                AllowEmployeeBookmark = group.AllowEmployeeBookmark,
                AllowEmployeeSticky = group.AllowEmployeeSticky,
                Channels = group.Channel.Select(ch => ChannelMapper.MapChannelEntityToModel(ch)).ToList(),
                Creator = group.CreatedByNavigation == null ? null : UserMapper.MapUserEntityToModel(group.CreatedByNavigation),
                Id = group.GroupId,
                IsDeleted = group.IsDeleted,
                Name = group.Name
            };
        }
    }
}
