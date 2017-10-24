using Business.Models;
using System;
using System.Collections.Generic;

namespace Business.Messages
{
    class MessageManager : IMessageManager
    {
        private readonly IMessageRepository messages;

        public MessageManager(IMessageRepository messages)
        {
            this.messages = messages;
        }

        public IMessage GetMessage(long id)
        {
            return messages.GetMessage(id);
        }

        public IMessage PostMessage(string content, long authorId)
        {
            User author = new User(authorId, null, null, null, null);
            Message message = new Message(null, content, author, null);

            return messages.AddMessage(message);
        }
    }
}
