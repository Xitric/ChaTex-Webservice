using Business.Errors;
using Business.Models;
using System;

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

        private void throwIfNotAdministrator(int groupId, int callerId)
        {
            GroupMembershipDetails membershipDetails = groupRepository.GetGroupMembershipDetailsForUser(groupId, callerId);

            if (!membershipDetails.IsAdministrator)
            {
                throw new InvalidArgumentException("The user must be an administrator of the group to perform this action", ParamNameType.CallerId);
            }
        }

        public void CreateChannel(int groupId, int callerId, string channelName)
        {
            throwIfNotAdministrator(groupId, callerId);

            channelRepository.CreateChannel(groupId, channelName);
        }

        public void DeleteChannel(int callerId, int channelId)
        {
            ChannelModel channel = channelRepository.GetChannel(channelId);

            if (channel == null)
            {
                throw new InvalidArgumentException("Channel does not exist", ParamNameType.ChannelId);
            }

            throwIfNotAdministrator(channel.GroupId, callerId);

            channelRepository.DeleteChannel(channelId);
        }

        public void UpdateChannel(int callerId, int channelId, string channelName)
        {
            ChannelModel channel = channelRepository.GetChannel(channelId);

            if (channel == null)
            {
                throw new InvalidArgumentException("Channel does not exist", ParamNameType.ChannelId);
            }

            throwIfNotAdministrator(channel.GroupId, callerId);

            channelRepository.UpdateChannel(new ChannelModel()
            {
                Id = channelId,
                Name = channelName
            });
        }
    }
}