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
            return new Channel()
            {
            };
        }

        public static ChannelModel MapChannelEntityToModel(Channel channel)
        {
            return new ChannelModel()
            {

            };
        }
    }
}
