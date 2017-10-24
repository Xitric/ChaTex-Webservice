using Models.Models;

namespace Models
{
    public interface IGroupRepository
    {
        void CreateGroup(IGroup group);

        bool DeleteGroup(long groupId);

    }
}
