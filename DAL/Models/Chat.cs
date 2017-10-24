using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Chat
    {
        public Chat()
        {
            ChatMessage = new HashSet<ChatMessage>();
            ChatUser = new HashSet<ChatUser>();
        }

        public int ChatId { get; set; }
        public string Name { get; set; }

        public ICollection<ChatMessage> ChatMessage { get; set; }
        public ICollection<ChatUser> ChatUser { get; set; }
    }
}
