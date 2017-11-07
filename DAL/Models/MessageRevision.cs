using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class MessageRevision
    {
        public int MessageId { get; set; }
        public DateTime EditDate { get; set; }
        public string Content { get; set; }

        public Message Message { get; set; }
    }
}
