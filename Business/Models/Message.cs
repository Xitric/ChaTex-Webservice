using System;

namespace Business.Models
{
    class Message : IMessage
    {
        public long? Id { get; }

        public string Content { get; }

        public IUser Author { get; }

        public DateTime? CreationTime { get; }

        public Message(long? id, string content, IUser author, DateTime? creationTime)
        {
            Id = id;
            Content = content;
            Author = author;
            CreationTime = creationTime;
        }
    }
}
