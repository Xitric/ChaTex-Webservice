using System;
using System.Collections.Generic;

namespace Business.Models
{
    public interface IGroup
    {
        long? Id { get; }
        string Name { get; }
        IUser Creator { get; }
        DateTime? CreationTime { get; }
        List<IChannel> Channels { get; }
    }
}
