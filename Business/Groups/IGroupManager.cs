using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Groups
{
    public interface IGroupManager
    {
        int? CreateGroup(int userId, string groupName, bool allowEmployeeSticky = false, bool allowEmployeeAcknowledgeable = false, bool allowEmployeeBookmark = false);

        bool DeleteGroup(int groupId, int callerId);

        void AddUsersToGroup(int groupId, List<int> userIds, int loggedInUser);

        void RemoveUsersFromGroup(int groupId, List<int> userIds, int loggedInUserId);

        void AddRolesToGroup(int groupId, int callerId, IEnumerable<int> roleIds);

        void RemoveRolesFromGroup(int groupId, int callerId, IEnumerable<int> roleIds);

        void SetUserAdministratorOnGroup(int groupId, int userId, int callerId, bool isAdministrator);

        IEnumerable<UserModel> GetAllGroupUsers(int groupId, int callerId);

        IEnumerable<GroupModel> GetGroupsForUser(int userId);

        void UpdateGroup(int groupId, string groupName);
    }
}
