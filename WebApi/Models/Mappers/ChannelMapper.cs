using Business.Models;
using IO.Swagger.Models;
using System;

namespace WebAPI.Models.Mappers
{
    class ChannelMapper
    {
        public static ChannelDTO MapChannelToChannelDTO(ChannelModel channel)
        {
            if (channel == null) return null;
            return new ChannelDTO()
            {
                Id = channel.Id,
                Name = channel.Name
            };
        }

        public static ChannelEventDTO MapChannelEventToChannelEventDTO(ChannelEventModel channelEventModel, int callerId)
        {
            if (channelEventModel == null) return null;
            return new ChannelEventDTO()
            {
                Type = MapChannelEventTypeToChannelEventTypeEnum(channelEventModel.Type),
                TimeOfOccurrence = channelEventModel.TimeOfOccurrence,
                Message = MessageMapper.MapMessageToGetMessageDTO(channelEventModel.Message, callerId),
                Channel = ChannelMapper.MapChannelToChannelDTO(channelEventModel.Channel)
            };
        }

        private static ChannelEventDTO.TypeEnum MapChannelEventTypeToChannelEventTypeEnum(ChannelEventType channelEventType)
        {
            return (ChannelEventDTO.TypeEnum)Enum.Parse(typeof(ChannelEventDTO.TypeEnum), channelEventType.ToString() + "Enum");
        }
    }
}
