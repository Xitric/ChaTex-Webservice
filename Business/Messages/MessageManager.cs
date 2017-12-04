using Business.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading;
using Business.Channels;
using Business.Errors;

namespace Business.Messages
{
    class MessageManager : IMessageManager
    {
        private readonly IMessageRepository messageRepository;
        private readonly IGroupRepository groupRepository;
        private readonly IChannelRepository channelRepository;
        private readonly ChannelEventManager channelEventManager;

        public MessageManager(IMessageRepository messageRepository, IGroupRepository groupRepository, IChannelRepository channelRepository, ChannelEventManager channelEventManager)
        {
            this.messageRepository = messageRepository;
            this.groupRepository = groupRepository;
            this.channelRepository = channelRepository;
            this.channelEventManager = channelEventManager;
        }

        private bool isUserInChannel(int userId, int channelId)
        {
            return groupRepository.GetGroupsForUser(userId)
                .Select(g => g.Channels.Any(c => c.Id == channelId))
                .Any();
        }

        public IEnumerable<MessageModel> GetMessages(int channelId, int callerId, DateTime before, int count)
        {
            bool hasAccess = isUserInChannel(callerId, channelId);

            if (!hasAccess)
            {
                throw new InvalidArgumentException("User does not have access to the specified channel", ParamNameType.CallerId);
            }

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

            if (isUserInChannel(callerId, message.ChannelId))
            {
                return message;
            }
            else
            {
                throw new InvalidArgumentException("Message could not be retrieved because the user does not have access to the channel containing the message", ParamNameType.CallerId);
            }
        }

        public int CreateMessage(int callerId, int channelId, string messageContent)
        {
            if (channelRepository.GetChannel(channelId) == null)
            {
                throw new InvalidArgumentException("The specified channel does not exist. Maybe it has been deleted.", ParamNameType.ChannelId);
            }

            if (!isUserInChannel(callerId, channelId))
            {
                throw new InvalidArgumentException("User does not have access to the specified channel", ParamNameType.CallerId);
            }

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
                    throw new InvalidArgumentException("User does not have the rights to delete the specified message", ParamNameType.CallerId);
                }
            }
            finally
            {
                channelEventManager.UnlockChannelForWrite((int)channel.Id);
            }
        }
    }
}
