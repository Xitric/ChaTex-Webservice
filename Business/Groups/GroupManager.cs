﻿using Models.Models;

namespace Business.Groups
{
    class GroupManager : IGroupManager
    {
        public void CreateGroup(long userId, string groupName, bool allowEmployeeSticky = false, bool allowEmployeeAcknowledgeable = false, bool allowEmployeeBookmark = false)
        {
            Group group = new Group()
            {
                Creator = new User()
                {
                    Id = userId
                },
                Name = groupName,
                AllowEmployeeSticky = allowEmployeeSticky,
                AllowEmployeeAcknowledgeable = allowEmployeeAcknowledgeable,
                AllowEmployeeBookmark = allowEmployeeBookmark
            };

            //TODO:

        }

        public bool DeleteGroup(long userId, bool isDeleted)
        {
            return true;
        }
    }
}
