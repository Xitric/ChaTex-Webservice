using Business.Models;
using System;
using System.Collections.Generic;

namespace Business
{
    public interface IMessageRepository
    {
        void CreateMessage(MessageModel message, int channelId);
    }
}
