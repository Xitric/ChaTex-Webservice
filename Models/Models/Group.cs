using System;
using System.Collections.Generic;

namespace Models.Models
{
    public class Group : IGroup
    {
        public long? Id { get; set; }

        public string Name { get; set; }

        public IUser Creator { get; set; }

        public DateTime? CreationTime { get; set; }

        public List<IChannel> Channels { get; set; }

        public bool AllowEmployeeSticky { get; set; }

        public bool AllowEmployeeAcknowledgeable { get; set; }

        public bool AllowEmployeeBookmark { get; set; }
    }
}
