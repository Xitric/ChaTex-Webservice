using Business;
using Business.Models;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<MessageModel> GetMessages(int channelId, int callerId, int from, int count)
        {
            bool hasChannelAccess = users.GetGroupsForUser(callerId)
                .Select(g => g.Channels.Any(c => c.Id == channelId))
                .Any();
            
            if (hasChannelAccess)
            {
                messages.getMessages(channelId, from, count);
            }

            return new List<MessageModel>();
        }
    }
}
