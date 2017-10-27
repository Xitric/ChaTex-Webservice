using Business.Models;
using System;
using System.Collections.Generic;

namespace Business
{
    public interface IMessageRepository
    {
        IEnumerable<MessageModel> getMessages(int channelId, int from, int count);
        IEnumerable<MessageModel> getMessagesSince(int channelId, DateTime since);
        void CreateMessage(MessageModel message, int channelId);
    }
}
