﻿using Business.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Business.Messages
{
    public interface IMessageManager
    {
        IEnumerable<MessageModel> GetMessages(int channelId, int callerId, DateTime before, int count);
        MessageModel GetMessage(int callerId, int messageId);
        int CreateMessage(int callerId, int channelId, string messageContent);
        void DeleteMessage(int callerId, int messageId);
        void EditMessage(int callerId, int messageId, string newContent);
    }
}
