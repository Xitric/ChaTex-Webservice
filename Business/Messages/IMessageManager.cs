using Business.Models;

namespace Business.Messages
{
    public interface IMessageManager
    {
        void PostMessage(string content, int authorId);

        MessageModel GetMessage(int id);
    }
}
