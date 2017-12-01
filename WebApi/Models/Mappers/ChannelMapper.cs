using Business.Models;
using IO.Swagger.Models;
using System;

namespace WebAPI.Models.Mappers
{
    class ChannelMapper
    {
        public static ChannelDTO MapChannelToChanelDTO(ChannelModel channel)
        {
            if (channel == null) return null;
            return new ChannelDTO(channel.Id, channel.Name);
        }

        public static ChannelEventDTO MapChannelEventToChannelEventDTO(ChannelEventModel channelEventModel, int callerId)
        {
            if (channelEventModel == null) return null;
            return new ChannelEventDTO()
            {
                Type = MapChannelEventTypeToChannelEventTypeEnum(channelEventModel.Type),
                Message = MessageMapper.MapMessageToGetMessageDTO(channelEventModel.Message, callerId)
            };
        }

        private static ChannelEventDTO.TypeEnum MapChannelEventTypeToChannelEventTypeEnum(ChannelEventType channelEventType)
        {
            return (ChannelEventDTO.TypeEnum)Enum.Parse(typeof(ChannelEventDTO.TypeEnum), channelEventType.ToString() + "Enum");
        }
    }
}
