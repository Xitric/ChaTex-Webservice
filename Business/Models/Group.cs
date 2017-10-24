using System;
using System.Collections.Generic;

namespace Business.Models
{
    class Group : IGroup
    {
        public long? Id { get; }

        public string Name { get; }

        public IUser Creator { get; }

        public DateTime? CreationTime { get; }

        public List<IChannel> Channels { get; }

        public Group(long? id, string name, IUser creator, DateTime? creationTime, List<IChannel> channels)
        {
            Id = id;
            Name = name;
            Creator = creator;
            CreationTime = creationTime;
            Channels.AddRange(channels);
        }
    }
}
