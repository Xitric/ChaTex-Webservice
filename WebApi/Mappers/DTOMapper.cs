using Business.Models;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Models;

namespace WebAPI.Mappers
{
    class DTOMapper
    {
        public GetMessageDTO ConvertMessage(MessageModel message)
        {
            return new GetMessageDTO(message.Id, message.CreationTime, message.Content, ConvertUser(message.Author));
        }

        public UserDTO ConvertUser(UserModel user)
        {
            return new UserDTO(user.Id, user.FirstName, user.MiddleInitial?.ToString(), user.LastName, user.Email);
        }

        public GroupDTO ConvertGroup(GroupModel group)
        {
            List<ChannelDTO> channels = group.Channels
                .Select(c => ConvertChannel(c))
                .ToList();

            return new GroupDTO(group.Id, group.Name, channels);
        }

        public ChannelDTO ConvertChannel(ChannelModel channel)
        {
            return new ChannelDTO(channel.Id, channel.Name);
        }
    }
}
