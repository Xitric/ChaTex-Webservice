using Business.Models;
using DAL.Models;

namespace DAL.Mapper
{
    class ChannelMapper
    {
        public static Channel MapChannelModelToEntity(ChannelModel channelModel)
        {
            if (channelModel == null)
            {
                return null;
            }

            return new Channel()
            {
                ChannelId = channelModel.Id == null ? 0 : channelModel.Id.Value,
                Name = channelModel.Name,
                IsDeleted = false, //TODO
                GroupId = channelModel.GroupId
            };
        }

        public static ChannelModel MapChannelEntityToModel(Channel channel)
        {
            if (channel == null)
            {
                return null;
            }

            return new ChannelModel()
            {
                Id = channel.ChannelId,
                Name = channel.Name,
                GroupId = channel.GroupId
            };
        }
    }
}
