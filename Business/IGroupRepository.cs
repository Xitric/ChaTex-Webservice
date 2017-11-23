using System.Collections.Generic;
using Business.Models;

namespace Business
{
    public interface IGroupRepository
    {
        int CreateGroup(GroupModel groupmodel);

        bool DeleteGroup(int groupId);

        void AddMemberToGroup(GroupUserModel groupUserModel);

        void AddMembersToGroup(IEnumerable<GroupUserModel> groupUserModel);

        void RemoveUsersFromGroup(IEnumerable<GroupUserModel> groupUserModel);

        void AddRolesToGroup(IEnumerable<GroupRoleModel> groupRoleModel);

        void RemoveRolesFromGroup(IEnumerable<GroupRoleModel> groupRoleModel);

        bool SetUserAdministratorOnGroup(GroupUserModel groupUserModel);
         
        GroupUserModel GetGroupUser(int groupId, int loggedInUser);

        IEnumerable<GroupModel> GetGroupsForUser(int userId);

        IEnumerable<UserModel> GetAllGroupUsers(int groupId);

        IEnumerable<UserModel> GetAllGroupAdmins(int groupId);
        void UpdateGroup(int groupId, string groupName, int callerId);

    }
}
 