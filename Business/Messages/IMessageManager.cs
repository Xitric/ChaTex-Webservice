using Business.Models;

namespace Business.Messages
{
    public interface IMessageManager
    {
        void PostMessage(string content, long authorId);

        MessageModel GetMessage(long id);
    }
}
