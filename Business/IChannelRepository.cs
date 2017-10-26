using Business.Models;

namespace Business
{
    public interface IChannelRepository
    {
        void CreateChannel(int groupId, string name);

        void DeleteChannel(int groupId, int channelId);

        void UpdateChannel(ChannelModel channelModel);
    }
}
