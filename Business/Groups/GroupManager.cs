using Business.Models;

namespace Business.Groups
{
    class GroupManager : IGroupManager
    {
        private readonly IGroupRepository groupRepository;
        public GroupManager(IGroupRepository groupRepository)
        {
            this.groupRepository = groupRepository;
        }

        public void CreateGroup(int userId, string groupName, bool allowEmployeeSticky = false, bool allowEmployeeAcknowledgeable = false, bool allowEmployeeBookmark = false)
        {
            GroupModel group = new GroupModel()
            {
                Creator = new UserModel()
                {
                    Id = userId
                },
                Name = groupName,
                AllowEmployeeSticky = allowEmployeeSticky,
                AllowEmployeeAcknowledgeable = allowEmployeeAcknowledgeable,
                AllowEmployeeBookmark = allowEmployeeBookmark
            };

            groupRepository.CreateGroup(group);

        }

        public bool DeleteGroup(int userId, bool isDeleted)
        {
            return true;
        }
    }
}
