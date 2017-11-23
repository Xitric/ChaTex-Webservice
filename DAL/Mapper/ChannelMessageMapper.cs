using Business.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Mapper
{
    class ChannelMessageMapper
    {
        public static ChannelMessages MapChannelMessageModelToEntity(ChannelMessageModel channelMessageModel)
        {
            if (channelMessageModel == null)
            {
                return null;
            }

            return new ChannelMessages()
            {
                Channel = ChannelMapper.MapChannelModelToEntity(channelMessageModel.Channel),
                Message = MessageMapper.MapMessageModelToEntity(channelMessageModel.Message)
            };
        }

        public static ChannelMessageModel MapChannelMessageEntityToModel(ChannelMessages channelMessage)
        {
            if (channelMessage == null)
            {
                return null;
            }

            return new ChannelMessageModel()
            {
            Channel = ChannelMapper.MapChannelEntityToModel(channelMessage.Channel),
            Message = MessageMapper.MapMessageEntityToModel(channelMessage.Message)
        };
    }
}
}
