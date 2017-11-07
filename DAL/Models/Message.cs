using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Message
    {
        public Message()
        {
            ChannelBookmark = new HashSet<ChannelBookmark>();
            ChannelMessages = new HashSet<ChannelMessages>();
            ChatMessage = new HashSet<ChatMessage>();
            UserSavedMessage = new HashSet<UserSavedMessage>();
        }

        public int MessageId { get; set; }
        public string Content { get; set; }
        public DateTime? LastEditDate { get; set; }
        public int UserId { get; set; }
        public DateTime? DeletionDate { get; set; }
        public DateTime CreationDate { get; set; }

        public User User { get; set; }
        public MessageRevision MessageRevision { get; set; }
        public ICollection<ChannelBookmark> ChannelBookmark { get; set; }
        public ICollection<ChannelMessages> ChannelMessages { get; set; }
        public ICollection<ChatMessage> ChatMessage { get; set; }
        public ICollection<UserSavedMessage> UserSavedMessage { get; set; }
    }
}
