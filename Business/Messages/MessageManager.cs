using Business.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading;
using Business.Channels;

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
                throw new ArgumentException("User does not have access to the specified channel", "callerId");
            }

            //There is no reason to use any locks in this method, as it does not matter if something happens in the channel while simply getting messages - it only matters when listening for events
            return messageRepository.GetMessages(channelId, before, count);
        }

        public MessageModel GetMessage(int callerId, int messageId)
        {
            //No need to use locks here either
            MessageModel message = messageRepository.GetMessage(messageId);
            if (message == null)
            {
                throw new ArgumentException("The requested message does not exist", "messageId");
            }

            if (isUserInChannel(callerId, message.ChannelId))
            {
                return message;
            }
            else
            {
                throw new ArgumentException("Message could not be retrieved because the user does not have access to the channel containing the message", "callerId");
            }
        }

        public void CreateMessage(int callerId, int channelId, string messageContent)
        {
            if (channelRepository.GetChannel(channelId) == null)
            {
                throw new ArgumentException("The specified channel does not exist. Maybe it has been deleted.", "channelId");
            }

            if (!isUserInChannel(callerId, channelId))
            {
                throw new ArgumentException("User does not have access to the specified channel", "callerId");
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
                messageRepository.CreateMessage(message, channelId);
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
                throw new ArgumentException("The message with the specified id does not exist", "messageId");
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
                    throw new ArgumentException("User does not have the rights to delete the specified message", "callerId");
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
            if (message == null)
            {
                throw new ArgumentException("Message with the specified id does not exist", "messageId");
            }

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
                    throw new ArgumentException("User does not have the rights to delete the specified message", "callerId");
                }
            }
            finally
            {
                channelEventManager.UnlockChannelForWrite((int)channel.Id);
            }
        }
    }
}
