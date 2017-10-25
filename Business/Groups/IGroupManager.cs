using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Groups
{
    public interface IGroupManager
    {
        void CreateGroup(int userId, string groupName, bool allowEmployeeSticky = false, bool allowEmployeeAcknowledgeable = false, bool allowEmployeeBookmark = false);

        bool DeleteGroup(int userId, Boolean isDeleted);
        void AddUsersToGroup(int groupId, List<int> userIds);
    }
}
