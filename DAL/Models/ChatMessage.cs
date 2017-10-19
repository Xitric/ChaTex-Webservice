using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class ChatMessage
    {
        public int ChatId { get; set; }
        public int MessageId { get; set; }

        public Chat Chat { get; set; }
        public Message Message { get; set; }
    }
}
