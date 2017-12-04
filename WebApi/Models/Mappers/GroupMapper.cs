using Business.Models;
using IO.Swagger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Mappers
{
    class GroupMapper
    {
        public static GroupDTO MapGroupToGroupDTO(GroupModel group)
        {
            List<ChannelDTO> channels = group.Channels
                .Select(c => ChannelMapper.MapChannelToChannelDTO(c))
                .ToList();

            return new GroupDTO()
            {
                Id = group.Id,
                Name = group.Name,
                Channels = channels
            };
        }
    }
}
