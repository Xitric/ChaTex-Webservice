using Business.Models;

namespace Business.Channels
{
    class ChannelManager : IChannelManager
    {
        private readonly IChannelRepository channelRepository;
        private readonly IGroupRepository groupRepository;

        public ChannelManager(IChannelRepository channelRepository, IGroupRepository groupRepository)
        {
            this.channelRepository = channelRepository;
            this.groupRepository = groupRepository;
        }

        public bool CreateChannel(int groupId, int callerId, string channelName)
        {
            GroupUserModel user = groupRepository.GetGroupUser(groupId, callerId);

            if (user.IsAdministrator)
            {
                channelRepository.CreateChannel(groupId, channelName);
                return true;
            }

            return false;
        }
    }
}
