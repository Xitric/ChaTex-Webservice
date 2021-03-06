﻿using Business.Models;
using System;
using System.Collections.Generic;

namespace Business
{
    public interface IMessageRepository
    {
        IEnumerable<MessageModel> GetMessages(int channelId, DateTime before, int count);
        IEnumerable<MessageModel> GetMessagesSince(int channelId, DateTime since);
        IEnumerable<MessageModel> GetDeletedMessagesSince(int channelId, DateTime since);
        IEnumerable<MessageModel> GetEditedMessagesSince(int channelId, DateTime since);
        MessageModel GetMessage(int messageId);
        int CreateMessage(MessageModel messagemodel, int channelId);
        void DeleteMessage(int messageId);
        void EditMessage(int messageId, string content);
    }
}
