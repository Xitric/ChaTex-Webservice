using Business.Errors;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Business.Channels
{
    class ChannelManager : IChannelManager
    {
        private readonly IChannelRepository channelRepository;
        private readonly IGroupRepository groupRepository;
        private readonly ChannelEventManager channelEventManager;

        public ChannelManager(IChannelRepository channelRepository, IGroupRepository groupRepository, ChannelEventManager channelEventManager)
        {
            this.channelRepository = channelRepository;
            this.groupRepository = groupRepository;
            this.channelEventManager = channelEventManager;
        }
        
        private void throwIfNotAdministrator(int groupId, int callerId)
        {
            GroupMembershipDetails membershipDetails = groupRepository.GetGroupMembershipDetailsForUser(groupId, callerId);

            if (!membershipDetails.IsAdministrator)
            {
                throw new InvalidArgumentException("The user must be an administrator of the group to perform this action", ParamNameType.CallerId);
            }
        }

        public int CreateChannel(int groupId, int callerId, string channelName)
        {
            throwIfNotAdministrator(groupId, callerId);

            return channelRepository.CreateChannel(groupId, channelName);
        }

        public void DeleteChannel(int callerId, int channelId)
        {
            ChannelModel channel = channelRepository.GetChannel(channelId);

            if (channel == null)
            {
                throw new InvalidArgumentException("Channel does not exist", ParamNameType.ChannelId);
            }

            throwIfNotAdministrator(channel.GroupId, callerId);

            channelEventManager.LockChannelForWrite(channelId);
            channelRepository.DeleteChannel(channelId);
            channelEventManager.UnlockChannelForWrite(channelId);
        }

        public void UpdateChannel(int callerId, int channelId, string channelName)
        {
            ChannelModel channel = channelRepository.GetChannel(channelId);

            if (channel == null)
            {
                throw new InvalidArgumentException("Channel does not exist", ParamNameType.ChannelId);
            }

            throwIfNotAdministrator(channel.GroupId, callerId);

            channelEventManager.LockChannelForWrite(channelId);
            channelRepository.UpdateChannel(new ChannelModel()
            {
                Id = channelId,
                Name = channelName
            });
            channelEventManager.UnlockChannelForWrite(channelId);
        }

        public IEnumerable<ChannelEventModel> GetChannelEvents(int channelId, int callerId, DateTime since, CancellationToken cancellation)
        {
            //TODO: Test if user has access to the channel

            return channelEventManager.GetChannelEvents(channelId, since, cancellation);
        }
    }
}