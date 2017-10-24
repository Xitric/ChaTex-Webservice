using System;
using System.Collections.Generic;

namespace Models.Models
{
    public interface IGroup
    {
        long? Id { get; set; }
        string Name { get; set; }
        IUser Creator { get; set; }
        DateTime? CreationTime { get; set; }
        List<IChannel> Channels { get; set; }
        bool AllowEmployeeSticky { get; set; }
        bool AllowEmployeeAcknowledgeable { get; set; }
        bool AllowEmployeeBookmark { get; set; }
    }
}
