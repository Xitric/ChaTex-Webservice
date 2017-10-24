using System;

namespace Models.Models
{
    public interface IMessage
    {
        long? Id { get; set; }
        string Content { get; set; }
        IUser Author { get; set; }
        DateTime? CreationTime { get; set; }
    }
}
