using System.Collections.Generic;
using Business.Models;

namespace Business
{
    public interface IGroupRepository
    {
        void CreateGroup(GroupModel group);

        bool DeleteGroup(int groupId);
        void AddMemberToGroup(GroupUserModel groupUserModel);
        void AddMembersToGroup(IEnumerable<GroupUserModel> groupUserModel);
        void RemoveUsersFromGroups(IEnumerable<GroupUserModel> groupUserModel);
        void AddRolesToGroup(IEnumerable<GroupRoleModel> groupRoleModel);
        void RemoveRolesFromGroup(IEnumerable<GroupRoleModel> groupRoleModel);
        void MarkUserAsAdministrator(GroupUserModel groupUserModel);
        GroupUserModel GetGroupUser(int groupId, int loggedInUser);
    }
}
