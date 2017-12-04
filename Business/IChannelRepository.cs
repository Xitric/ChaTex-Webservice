using Business.Models;
using System;
using System.Collections.Generic;

namespace Business
{
    public interface IChannelRepository
    {
        int CreateChannel(int groupId, string name);

        void DeleteChannel(int channelId);

        void UpdateChannel(ChannelModel channelModel);

        ChannelModel GetChannel(int channelId);

        DateTime? GetChannelDeletionDate(int channelId);

        DateTime? GetChannelRenameDate(int channelId);

        IEnumerable<ChannelModel> GetChannelDeletionsSince(DateTime since);

        IEnumerable<ChannelModel> GetChannelRenamesSince(DateTime since);
    }
}
