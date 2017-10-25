namespace Business.Channels
{
    public interface IChannelManager
    {
        bool CreateChannel(int groupId, int callerId, string channelName);
    }
}
