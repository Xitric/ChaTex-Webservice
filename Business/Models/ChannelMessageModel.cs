using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class ChannelMessageModel
    {
        public ChannelModel Channel { get; set; }
        public MessageModel Message { get; set; }
    }
}
