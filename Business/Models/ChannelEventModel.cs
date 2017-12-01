using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    /// <summary>
    /// Wrapper for an event that has happened in a channel. In the case of a delete- or rename channel event the Message property will be null.
    /// </summary>
    public class ChannelEventModel
    {
        public ChannelEventType Type { get; set; }
        public MessageModel Message { get; set; }
    }

    public enum ChannelEventType
    {
        NewMessage,
        UpdateMessage,
        DeleteMessage,
        RenameChannel,
        DeleteChannel
    }
}
