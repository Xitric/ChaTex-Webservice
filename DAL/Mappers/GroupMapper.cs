using Business.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Mappers
{
    class GroupMapper : IGroup
    {
        private readonly Group group;

        public GroupMapper(Group group)
        {
            this.group = group;
        }

        public long? Id => group.GroupId;

        public string Name => group.Name;

        public IUser Creator => new UserMapper(group.CreatedByNavigation);

        public DateTime? CreationTime => group.CreationDate;

        public List<IChannel> Channels => group.Channel.Select(c => new ChannelMapper(c)).ToList<IChannel>();
    }
}
