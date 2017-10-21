using Business.Models;
using DAL.Models;
using System;

namespace DAL.Mappers
{
    class ChannelMapper : IChannel
    {
        private readonly Channel channel;

        public ChannelMapper(Channel channel)
        {
            this.channel = channel;
        }

        public long? Id => channel.ChannelId;

        public string Name => channel.Name;
    }
}
