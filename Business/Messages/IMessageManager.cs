using Business.Models;

namespace Business.Messages
{
    public interface IMessageManager
    {
        IMessage PostMessage(IMessage message);

        IMessage GetMessage(long id);
    }
}
