using Business.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Business.Messages
{
    public interface IMessageManager
    {
        IEnumerable<MessageModel> GetMessages(int channelId, int callerId, int from, int count);
        IEnumerable<MessageModel> GetMessagesSince(int channelId, int callerId, DateTime since, CancellationToken cancellation);
        void CreateMessage(int callerId, int channelId, string messageContent);
    }
}
