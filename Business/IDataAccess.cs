using Business.Models;
using System;
using System.Collections.Generic;

namespace Business
{
    public interface IDataAccess
    {
        IMessage AddMessage(IMessage message);

        IMessage GetMessage(long id);

        List<IMessage> getMessagesSince(DateTime since);
    }
}
