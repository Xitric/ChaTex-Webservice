using System;

namespace Business.Models
{
    public interface IMessage
    {
        long? Id { get; }
        string Content { get; }
        IUser Author { get; }
        DateTime? CreationTime { get; }
    }
}
