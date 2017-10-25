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
    }
}
