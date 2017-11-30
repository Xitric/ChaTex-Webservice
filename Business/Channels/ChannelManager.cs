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

        public int CreateChannel(int groupId, int callerId, string channelName)
        {
            GroupUserModel user = groupRepository.GetGroupUser(groupId, callerId);

            if (user == null)
            {
                throw new InvalidArgumentException("User does not exist in group", ParamNameType.GroupId);
            }

            if (user.IsAdministrator)
            {
                return channelRepository.CreateChannel(groupId, channelName);
            }
            else
            {
                throw new InvalidArgumentException("User is not administrator in group", ParamNameType.CallerId);
            }
        }

        public void DeleteChannel(int callerId, int channelId)
        {
            ChannelModel channel = channelRepository.GetChannel(channelId);

            if (channel == null)
            {
                throw new InvalidArgumentException("Channel does not exist", ParamNameType.ChannelId);
            }

            GroupUserModel user = groupRepository.GetGroupUser(channel.GroupId, callerId);

            if (user == null)
            {
                throw new InvalidArgumentException("User does not exist in group", ParamNameType.GroupId);
            }

            if (user.IsAdministrator)
            {
                channelRepository.DeleteChannel(channelId);
            }
            else
            {
                throw new InvalidArgumentException("User is not administrator in group", ParamNameType.CallerId);
            }
        }

        public void UpdateChannel(int callerId, int channelId, string channelName)
        {
            ChannelModel channel = channelRepository.GetChannel(channelId);

            if (channel == null)
            {
                throw new InvalidArgumentException("Channel does not exist", ParamNameType.ChannelId);
            }

            GroupUserModel user = groupRepository.GetGroupUser(channel.GroupId, callerId);

            if (user == null)
            {
                throw new InvalidArgumentException("User does not exist in group", ParamNameType.GroupId);
            }

            if (user.IsAdministrator) {
                channelRepository.UpdateChannel(new ChannelModel()
                {
                    Id = channelId,
                    Name = channelName
                });
            }
            else
            {
                throw new InvalidArgumentException("User is not administrator in group", ParamNameType.CallerId);
            }
        }
    }
}