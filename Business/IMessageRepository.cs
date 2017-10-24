using Business.Models;
using System;
using System.Collections.Generic;

namespace Business
{
    public interface IMessageRepository
    {
        void AddMessage(MessageModel message);

        MessageModel GetMessage(long id);

        List<MessageModel> GetMessagesSince(DateTime since);
    }
}
