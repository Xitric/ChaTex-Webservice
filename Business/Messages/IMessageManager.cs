using Business.Models;
using System.Collections.Generic;

namespace Business.Messages
{
    public interface IMessageManager
    {
        IEnumerable<MessageModel> GetMessages(int channelId, int callerId, int from, int count);

        void CreateMessage(int groupId, int callerId, int channelId, string messageContent);
    }
}
