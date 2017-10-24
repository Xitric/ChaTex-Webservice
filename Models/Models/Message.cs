using System;

namespace Models.Models
{
    public class Message : IMessage
    {
        public long? Id { get; set; }

        public string Content { get; set; }

        public IUser Author { get; set; }

        public DateTime? CreationTime { get; set; }
    }
}
