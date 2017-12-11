using Business.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading;
using Business.Channels;
using Business.Errors;

namespace Business.Messages
{
    class MessageManager : AuthenticatedManager, IMessageManager
    {
        private readonly IMessageRepository messageRepository;
        private readonly IGroupRepository groupRepository;
        private readonly IChannelRepository channelRepository;
        private readonly ChannelEventManager channelEventManager;

        public MessageManager(IMessageRepository messageRepository, IGroupRepository groupRepository, IChannelRepository channelRepository, ChannelEventManager channelEventManager) : base(groupRepository)
        {
            this.messageRepository = messageRepository;
            this.groupRepository = groupRepository;
            this.channelRepository = channelRepository;
            this.channelEventManager = channelEventManager;
        }

        public IEnumerable<MessageModel> GetMessages(int channelId, int callerId, DateTime before, int count)
        {
            throwIfNoAccessToChannel(channelId, callerId);

            //There is no reason to use any locks in this method, as it does not matter if something happens in the channel while simply getting messages - it only matters when listening for events
            return messageRepository.GetMessages(channelId, before.ToUniversalTime(), count);
        }

        public MessageModel GetMessage(int callerId, int messageId)
        {
            //No need to use locks here either
            MessageModel message = messageRepository.GetMessage(messageId);
            if (message == null)
            {
                throw new InvalidArgumentException("The requested message does not exist", ParamNameType.MessageId);
            }

            throwIfNoAccessToChannel(message.ChannelId, callerId);

            return message;
        }

        public int CreateMessage(int callerId, int channelId, string messageContent)
        {
            if (channelRepository.GetChannel(channelId) == null)
            {
                throw new InvalidArgumentException("The specified channel does not exist. Maybe it has been deleted.", ParamNameType.ChannelId);
            }

            throwIfNoAccessToChannel(channelId, callerId);

            MessageModel message = new MessageModel()
            {
                Author = new UserModel { Id = callerId },
                Content = messageContent,
            };

            //Wait until we are allowed to add new messages
            channelEventManager.LockChannelForWrite(channelId);
            try
            {
                return messageRepository.CreateMessage(message, channelId);
            }
            finally
            {
                channelEventManager.UnlockChannelForWrite(channelId);
            }
        }

        public void DeleteMessage(int callerId, int messageId)
        {
            //Since message ids are fixed and messages don't change channels, these operations did not need to be within the lock
            MessageModel message = messageRepository.GetMessage(messageId);

            if (message == null)
            {
                throw new InvalidArgumentException("The message with the specified id does not exist", ParamNameType.MessageId);
            }

            var channel = channelRepository.GetChannel(message.ChannelId);
            if (channel == null) throw new InvalidArgumentException("This message no longer exists", ParamNameType.MessageId);

            channelEventManager.LockChannelForWrite((int)channel.Id);
            try
            {
                GroupMembershipDetails membershipDetails = groupRepository.GetGroupMembershipDetailsForUser(channel.GroupId, callerId);

                if (membershipDetails.IsAdministrator || message.Author.Id == callerId)
                {
                    messageRepository.DeleteMessage(messageId);
                }
                else
                {
                    throw new InvalidArgumentException("User does not have the rights to delete the specified message", ParamNameType.CallerId);
                }
            }
            finally
            {
                channelEventManager.UnlockChannelForWrite((int)channel.Id);
            }
        }

        public void EditMessage(int callerId, int messageId, string newContent)
        {
            //Since message ids are fixed and messages don't change channels, these operations did not need to be within the lock
            MessageModel message = messageRepository.GetMessage(messageId);

            if (message == null) throw new InvalidArgumentException("Message with the specified id does not exist", ParamNameType.MessageId);

            var channel = channelRepository.GetChannel(message.ChannelId);
            if (channel == null) throw new InvalidArgumentException("This message no longer exists", ParamNameType.MessageId);

            channelEventManager.LockChannelForWrite((int)channel.Id);
            try
            {
                GroupMembershipDetails membershipDetails = groupRepository.GetGroupMembershipDetailsForUser(channel.GroupId, callerId);

                if (membershipDetails.IsAdministrator || message.Author.Id == callerId)
                {
                    messageRepository.EditMessage(messageId, newContent);
                }
                else
                {
                    throw new InvalidArgumentException("User does not have the rights to edit the specified message", ParamNameType.CallerId);
                }
            }
            finally
            {
                channelEventManager.UnlockChannelForWrite((int)channel.Id);
            }
        }
    }
}
