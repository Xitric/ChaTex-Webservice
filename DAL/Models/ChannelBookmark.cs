using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class ChannelBookmark
    {
        public int ChannelId { get; set; }
        public int MessageId { get; set; }

        public Channel Channel { get; set; }
        public Message Message { get; set; }
    }
}
