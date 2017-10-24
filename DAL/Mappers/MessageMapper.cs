using Business.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Mappers
{
    class MessageMapper : IMessage
    {
        private readonly Message message;

        public MessageMapper(Message message)
        {
            this.message = message;
        }

        public long? Id => message.MessageId;

        public string Content => message.Content;

        public IUser Author => new UserMapper(message.User);

        public DateTime? CreationTime => message.CreationDate;
    }
}
