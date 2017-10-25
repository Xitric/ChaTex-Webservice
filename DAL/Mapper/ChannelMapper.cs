using Business.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Mapper
{
    class ChannelMapper
    {
        public static Channel MapChannelModelToEntity(ChannelModel channelModel)
        {
            if (channelModel == null) return null;
            return new Channel()
            {
                ChannelId = channelModel.Id == null ? 0 : channelModel.Id.Value,
                Name = channelModel.Name,
                IsDeleted = false, //TODO
                GroupId = 0 //TODO
            };
        }

        public static ChannelModel MapChannelEntityToModel(Channel channel)
        {
            if (channel == null) return null;
            return new ChannelModel()
            {
                Id = channel.ChannelId,
                Name = channel.Name
            };
        }
    }
}
