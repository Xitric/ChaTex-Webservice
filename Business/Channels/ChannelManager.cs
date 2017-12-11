using Business.Errors;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Business.Channels
{
    class ChannelManager : AuthenticatedManager, IChannelManager
    {
        private readonly IChannelRepository channelRepository;
        private readonly IGroupRepository groupRepository;
        private readonly ChannelEventManager channelEventManager;

        public ChannelManager(IChannelRepository channelRepository, IGroupRepository groupRepository, ChannelEventManager channelEventManager) : base(groupRepository)
        {
            this.channelRepository = channelRepository;
            this.groupRepository = groupRepository;
            this.channelEventManager = channelEventManager;
        }

        public int CreateChannel(int groupId, int callerId, string channelName)
        {
            //If the user is an administrator of the group when the method was called, we allow the operation to finish, even if the administrator status is revoked during method execution. Alternatively, we would block attempts at revoking administrator status, which has the same effect while being slower.
            throwIfNotAdministrator(groupId, callerId);

            return channelRepository.CreateChannel(groupId, channelName);
        }

        public void DeleteChannel(int callerId, int channelId)
        {
            //We use a write lock to ensure that others do not delete the channel while this thread makes an attempt to do so
            try
            {
                channelEventManager.LockChannelForWrite(channelId);
                deleteChannelInternal(callerId, channelId);
            }
            finally
            {
                channelEventManager.UnlockChannelForWrite(channelId);
            }
        }

        /// <summary>
        /// Internal, non-threadsafe method for deleting a channel. Synchronization is expected to happen elsewhere.
        /// </summary>
        /// <param name="callerId">The id of the user who made this request</param>
        /// <param name="channelId">The id of the channel to delete</param>
        private void deleteChannelInternal(int callerId, int channelId)
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
            try
            {
                channelEventManager.LockChannelForWrite(channelId);
                updateChannelInternal(callerId, channelId, channelName);
            }
            finally
            {
                channelEventManager.UnlockChannelForWrite(channelId);
            }
        }

        /// <summary>
        /// Internal, non-threadsafe method for renaming a channel. Synchronization is expected to happen elsewhere.
        /// </summary>
        /// <param name="callerId">The id of the user who made this request</param>
        /// <param name="channelId">The id of the channel to rename</param>
        /// <param name="channelName">The new name for the channel</param>
        private void updateChannelInternal(int callerId, int channelId, string channelName)
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

        public IEnumerable<ChannelEventModel> GetChannelEvents(int channelId, int callerId, DateTime since, CancellationToken cancellation)
        {
            throwIfNoAccessToChannel(channelId, callerId);

            return channelEventManager.GetChannelEvents(channelId, since, cancellation);
        }
    }
}