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
            //Also, each method in the repository is expected to be threadsafe
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
            //We lock here to ensure that messages are not added until the channel is ready, and to ensure that the channel is not deleted while we add a message
            try
            {
                channelEventManager.LockChannelForWrite(channelId);
                return createMessageInternal(callerId, channelId, messageContent);
            }
            finally
            {
                channelEventManager.UnlockChannelForWrite(channelId);
            }
        }

        /// <summary>
        /// Internal, non-threadsafe method for creating a new message. This method expects synchronization to happen elsewhere.
        /// </summary>
        private int createMessageInternal(int callerId, int channelId, string messageContent)
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

            return messageRepository.CreateMessage(message, channelId);
        }

        public void DeleteMessage(int callerId, int messageId)
        {
            //This is threadsafe, as messages don't change channel ids over time
            var channel = getChannelForMessage(messageId);

            try
            {
                channelEventManager.LockChannelForWrite((int)channel.Id);
                deleteMessageInternal(callerId, messageId);
            }
            finally
            {
                channelEventManager.UnlockChannelForWrite((int)channel.Id);
            }
        }

        /// <summary>
        /// Internal, non-threadsafe method for deleting a message. This method expects synchronization to happen elsewhere.
        /// </summary>
        private void deleteMessageInternal(int callerId, int messageId)
        {
            throwIfNotAllowedToModifyMessage(callerId, messageId);

            messageRepository.DeleteMessage(messageId);
        }

        public void EditMessage(int callerId, int messageId, string newContent)
        {
            var channel = getChannelForMessage(messageId);

            try
            {
                channelEventManager.LockChannelForWrite((int)channel.Id);
                editMessageInternal(callerId, messageId, newContent);
            }
            finally
            {
                channelEventManager.UnlockChannelForWrite((int)channel.Id);
            }
        }

        /// <summary>
        /// Internal, non-threadsafe method for editing a message. This method expects synchronization to happen elsewhere.
        /// </summary>
        private void editMessageInternal(int callerId, int messageId, string newContent)
        {
            throwIfNotAllowedToModifyMessage(callerId, messageId);

            messageRepository.EditMessage(messageId, newContent);
        }

        /// <summary>
        /// Throw an InvalidArgumentException if the user with the specified id does not have the rights to modify the message with the specified id. This method should only be used inside a synchronized context.
        /// </summary>
        /// <param name="callerId">The id of the user to test for</param>
        /// <param name="messageId">The id of the message to test for</param>
        /// <exception cref="InvalidArgumentException">If the user does not have the rights to modify the message</exception>
        private void throwIfNotAllowedToModifyMessage(int callerId, int messageId)
        {
            var channel = getChannelForMessage(messageId);
            var membershipDetails = groupRepository.GetGroupMembershipDetailsForUser(channel.GroupId, callerId);
            var author = getAuthorForMessage(messageId);

            if (!membershipDetails.IsAdministrator && author.Id != callerId)
            {
                throw new InvalidArgumentException("User does not have the rights to modify the specified message", ParamNameType.CallerId);
            }
        }

        /// <summary>
        /// Get the channel that contains the specified message. This method should only be used inside a synchronized context.
        /// </summary>
        /// <param name="messageId">The id of the message</param>
        /// <returns>The channel that contains the specified message</returns>
        /// <exception cref="InvalidArgumentException">If the message does not exist</exception>
        private ChannelModel getChannelForMessage(int messageId)
        {
            MessageModel message = messageRepository.GetMessage(messageId);

            if (message == null)
            {
                throw new InvalidArgumentException("The message with the specified id does not exist", ParamNameType.MessageId);
            }

            var channel = channelRepository.GetChannel(message.ChannelId);
            if (channel == null)
            {
                throw new InvalidArgumentException("This message no longer exists", ParamNameType.MessageId);
            }

            return channel;
        }

        /// <summary>
        /// Get the author of the specified message. This method should only be used inside a synchronized context.
        /// </summary>
        /// <param name="messageId">The id of the message</param>
        /// <returns>The author of the message with the specified id</returns>
        /// <exception cref="InvalidArgumentException">If the message does not exist</exception>
        private UserModel getAuthorForMessage(int messageId)
        {
            MessageModel message = messageRepository.GetMessage(messageId);

            if (message == null)
            {
                throw new InvalidArgumentException("The message with the specified id does not exist", ParamNameType.MessageId);
            }

            return message.Author;
        }
    }
}
