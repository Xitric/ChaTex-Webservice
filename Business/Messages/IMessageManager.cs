using Business.Models;

namespace Business.Messages
{
    public interface IMessageManager
    {
        void CreateMessage(int groupId, int callerId, int channelId, string messageContent);
    }
}
