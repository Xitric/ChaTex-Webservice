using Models.Models;

namespace Business.Messages
{
    public interface IMessageManager
    {
        IMessage PostMessage(string content, long authorId);

        IMessage GetMessage(long id);
    }
}
