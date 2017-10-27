using Business.Models;
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
                .Select(c => ChannelMapper.MapChannelToChanelDTO(c))
                .ToList();

            return new GroupDTO(group.Id, group.Name, channels);
        }
    }
}
