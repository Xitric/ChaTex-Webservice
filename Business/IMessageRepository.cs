using Business.Models;
using System;
using System.Collections.Generic;

namespace Business
{
    public interface IMessageRepository
    {
        IEnumerable<MessageModel> GetMessages(int channelId, int from, int count);
        IEnumerable<MessageModel> getMessagesSince(int channelId, DateTime since);
        MessageModel GetMessage(int messageId);
        void CreateMessage(MessageModel message, int channelId);

        void DeleteMessage(int messageId);
    }
}
