using Business.Models;
using IO.Swagger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Mappers
{
    class ChannelMapper
    {
        public static ChannelDTO MapChannelToChanelDTO(ChannelModel channel)
        {
            return new ChannelDTO(channel.Id, channel.Name);
        }
    }
}
