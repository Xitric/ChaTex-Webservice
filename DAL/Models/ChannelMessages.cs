using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class ChannelMessages
    {
        public int MessageId { get; set; }
        public int ChannelId { get; set; }

        public Channel Channel { get; set; }
        public Message Message { get; set; }
    }
}
