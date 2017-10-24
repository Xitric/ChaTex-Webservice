using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class UserSavedMessage
    {
        public int UserId { get; set; }
        public int MessageId { get; set; }
        public bool? IsDeleted { get; set; }

        public Message Message { get; set; }
        public User User { get; set; }
    }
}
