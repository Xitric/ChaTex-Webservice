using Business.Models;

namespace Business
{
    public interface IGroupRepository
    {
        void CreateGroup(GroupModel group);

        bool DeleteGroup(long groupId);

    }
}
