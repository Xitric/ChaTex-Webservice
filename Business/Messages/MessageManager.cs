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

        public MessageModel GetMessage(int id)
        {
            return messages.GetMessage(id);
        }

        public void PostMessage(string content, int authorId)
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
