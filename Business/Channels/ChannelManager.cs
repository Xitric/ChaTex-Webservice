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

        public bool DeleteChannel(int groupId, int callerId, int channelId)
        {
            GroupUserModel user = groupRepository.GetGroupUser(groupId, callerId);

            if (user.IsAdministrator)
            {
                channelRepository.DeleteChannel(groupId, channelId);
                return true;
            }
            return false;
        }

        public bool UpdateChannel(int groupId, int callerId, int channelId, string channelName)
        {
            GroupUserModel user = groupRepository.GetGroupUser(groupId, callerId);

            if (user.IsAdministrator) {
                channelRepository.UpdateChannel(new ChannelModel()
                {
                    Id = channelId,
                    Name = channelName
                });
                return true;
            }

            return false;
        }
        
    }
}
