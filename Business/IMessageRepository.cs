using Business.Models;
using System;
using System.Collections.Generic;

namespace Business
{
    public interface IMessageRepository
    {
        IMessage AddMessage(IMessage message);

        IMessage GetMessage(long id);

        List<IMessage> GetMessagesSince(DateTime since);
    }
}
