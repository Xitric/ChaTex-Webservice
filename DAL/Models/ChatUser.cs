using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class ChatUser
    {
        public int ChatId { get; set; }
        public int UserId { get; set; }

        public Chat Chat { get; set; }
        public User User { get; set; }
    }
}
