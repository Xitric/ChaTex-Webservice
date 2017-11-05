using Business.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading;

namespace Business.Messages
{
    class MessageManager : IMessageManager
    {
        private readonly IMessageRepository messages;
        private readonly IGroupRepository groups;
        private readonly IChannelRepository channels;

        //These locks are used to ensure that two threads to not read and write messages simultaneously
        private readonly Dictionary<int, object> messageLocks;
        private readonly int sleepInterval = 2000;

        private object GetLockForChannel(int channelId)
        {
            object msgLock = messageLocks.GetValueOrDefault(channelId);

            if (msgLock == null)
            {
                //No lock exists, so make a new one
                msgLock = new object();
                bool success = messageLocks.TryAdd(channelId, msgLock);

                if (!success)
                {
                    //This indicates that another thread was faster, so use the existing lock
                    msgLock = messageLocks[channelId];
                }
            }

            return msgLock;
        }

        public MessageManager(IMessageRepository messages, IGroupRepository groups, IChannelRepository channels)
        {
            this.messages = messages;
            this.groups = groups;
            this.channels = channels;
            messageLocks = new Dictionary<int, object>();
        }

        private bool IsUserInChannel(int userId, int channelId)
        {
            return groups.GetGroupsForUser(userId)
                .Select(g => g.Channels.Any(c => c.Id == channelId))
                .Any();
        }

        public IEnumerable<MessageModel> GetMessages(int channelId, int callerId, int from, int count)
        {
            if (IsUserInChannel(callerId, channelId))
            {
                //This should be threadsafe
                return messages.GetMessages(channelId, from, count);
            }

            return new List<MessageModel>();
        }

        public void CreateMessage(int callerId, int channelId, string messageContent)
        {
            if (IsUserInChannel(callerId, channelId))
            {
                MessageModel message = new MessageModel()
                {
                    Author = new UserModel { Id = callerId },
                    Content = messageContent,
                };

                //Waith until we are allowed to add new messages
                object msgLock;
                lock (msgLock = GetLockForChannel(channelId))
                {
                    messages.CreateMessage(message, channelId);
                    Console.WriteLine($"New message for channel {channelId}, wake up my little lambs!");
                    Monitor.PulseAll(msgLock);
                }
            }
            else
            {
                throw new Exception("Message couldnt be created, user isnt in the channel group");
            }
        }

        public void DeleteMessage(int callerId, int messageId)
        {
            MessageModel message = messages.GetMessage(messageId);
            var channel = channels.GetChannel(message.ChannelId);
            var loggedInUser = groups.GetGroupUser(channel.GroupId, callerId);
            if (loggedInUser.IsAdministrator)
            {
                messages.DeleteMessage(messageId);
            }
            else if(message.Author.Id == callerId)
            {
                messages.DeleteMessage(messageId);
            } else
            {
                throw new Exception("You dosent have the rights to delete this message!");
            }

        }

        public MessageModel GetMessage(int callerId, int messageId)
        {
            MessageModel message = messages.GetMessage(messageId);
            if(message == null)
            {
                throw new Exception("Message does not exist in the database");
            }
            if (IsUserInChannel(callerId, message.ChannelId))
            {
                return message;
            }
            else
            {
                throw new Exception("Message couldnt be retrived because the user isnt in the channel group");
            }
        }

        /// <summary>
        /// Request events about messages in the specified channel. This method will block until a new message has been posted, a message has been deleted, or a message has been edited.
        /// </summary>
        /// <param name="channelId">The channel to wait for events in</param>
        /// <param name="callerId">The id of the client making this request</param>
        /// <param name="since">The timestamp from which to get events</param>
        /// <param name="cancellation">Token specifying if this blocking method should be cancelled</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The channel with the specified id does not exist, or the caller does not have access to the specified channel</exception>
        public IEnumerable<MessageEventModel> GetMessageEvents(int channelId, int callerId, DateTime since, CancellationToken cancellation)
        {
            return null;
        }
    }
}
