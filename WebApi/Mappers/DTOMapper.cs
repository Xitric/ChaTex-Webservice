using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Models;

namespace WebAPI.Mappers
{
    class DTOMapper
    {
        public GetMessage ConvertMessage(IMessage message)
        {
            return new GetMessage(message.Id, message.CreationTime, message.Content, ConvertUser(message.Author));
        }

        public User ConvertUser(IUser user)
        {
            return new User(user.Id, user.FirstName, user.MiddleInitial?.ToString(), user.LastName, user.Email);
        }

        public Group ConvertGroup(IGroup group)
        {
            List<Channel> channels = group.Channels
                .Select(c => ConvertChannel(c))
                .ToList();

            return new Group(group.Id, group.Name, channels);
        }

        public Channel ConvertChannel(IChannel channel)
        {
            return new Channel(channel.Id, channel.Name);
        }
    }
}
