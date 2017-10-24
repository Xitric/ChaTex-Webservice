using Models;
using Models.Models;

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
            User author = new User()
            {
                Id = authorId
            };
            Message message = new Message()
            {
                Content = content,
                Author = author
            };

            return messages.AddMessage(message);
        }
    }
}
