using Business.Models;
using System.Collections.Generic;

namespace Business
{
    public interface IMessageRepository
    {
        IEnumerable<MessageModel> getMessages(int channelId, int from, int count);
        void CreateMessage(MessageModel message, int channelId);
    }
}
