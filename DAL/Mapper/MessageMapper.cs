using Business.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Mapper
{
    class MessageMapper
    {
        public static Message MapMessageModelToEntity(MessageModel messageModel)
        {
            if (messageModel == null) return null;
            return new Message()
            {
                MessageId = messageModel.Id == null ? 0 : messageModel.Id.Value,
                Content = messageModel.Content,
                UserId = (int)messageModel.Author.Id,
            };
        }

        public static MessageModel MapMessageEntityToModel(Message message)
        {
            if (message == null) return null;
            var owningChannel = message.ChannelMessages.FirstOrDefault(x => x.MessageId == message.MessageId);
            return new MessageModel()
            {
                Author = UserMapper.MapUserEntityToModel(message.User),
                Content = message.Content,
                CreationTime = message.CreationDate,
                DeletionTime = message.DeletionDate,
                LastEdited = message.LastEditDate,
                Id = message.MessageId,
                ChannelId = owningChannel == null ? 0 : owningChannel.ChannelId
            };
        }
    }
}
