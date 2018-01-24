using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class ChannelEvent
    {
        public byte EventType { get; set; }
        public DateTime TimeOfOccurrence { get; set; }
        public int ChannelId { get; set; }

        public Channel Channel { get; set; }
    }
}
