using Business.Models;
using System.Collections.Generic;

namespace Business.Messages
{
    public interface IMessageManager
    {
        IEnumerable<MessageModel> GetMessages(int groupId, int channelId, int callerId, int from, int count);
    }
}
