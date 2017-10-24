using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Groups
{
    public interface IGroupManager
    {
        void CreateGroup(long userId, string groupName, bool allowEmployeeSticky = false, bool allowEmployeeAcknowledgeable = false, bool allowEmployeeBookmark = false);

        bool DeleteGroup(long userId, Boolean isDeleted);
    }
}
