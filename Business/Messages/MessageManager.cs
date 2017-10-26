using Business.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Business.Messages
{
    class MessageManager : IMessageManager
    {
        private readonly IMessageRepository messages;
        private readonly IUserRepository users;

        public MessageManager(IMessageRepository messages, IUserRepository users)
        {
            this.messages = messages;
            this.users = users;
        }

        private bool IsUserInChannel(int userId, int channelId)
        {
            return users.GetGroupsForUser(userId)
                .Select(g => g.Channels.Any(c => c.Id == channelId))
                .Any();
        }

        public IEnumerable<MessageModel> GetMessages(int channelId, int callerId, int from, int count)
        {
            if (IsUserInChannel(callerId, channelId))
            {
                messages.getMessages(channelId, from, count);
            }

            return new List<MessageModel>();
        }
        public void CreateMessage(int groupId, int callerId, int channelId, string messageContent)
        {
            if (IsUserInChannel(callerId, channelId))
            {
                messages.CreateMessage(new MessageModel()
                {
                    Author = new UserModel { Id = callerId },
                    Content = messageContent,
                }, channelId);
            } else
            {
                throw new Exception("Message couldnt be created, user isnt in the channel group");
            }
        }
    }
}
