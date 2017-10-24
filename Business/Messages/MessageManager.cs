using Business;
using Business.Models;

namespace Business.Messages
{
    class MessageManager : IMessageManager
    {
        private readonly IMessageRepository messages;

        public MessageManager(IMessageRepository messages)
        {
            this.messages = messages;
        }

        public MessageModel GetMessage(long id)
        {
            return messages.GetMessage(id);
        }

        public void PostMessage(string content, long authorId)
        {
            UserModel author = new UserModel()
            {
                Id = authorId
            };
            MessageModel message = new MessageModel()
            {
                Content = content,
                Author = author
            };

            messages.AddMessage(message);
        }
    }
}
