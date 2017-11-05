using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class MessageEventModel
    {
        public MessageEventType Type { get; set; }
        public MessageModel Message { get; set; }
    }

    public enum MessageEventType
    {
        NewMessage,
        UpdateMessage,
        DeleteMessage
    }
}
