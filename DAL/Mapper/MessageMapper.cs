using Business.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
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
                Content = messageModel.Content,
                UserId = (int)messageModel.Author.Id,
                IsDeleted = messageModel.isDeleted,
                MessageId = messageModel.Id == null ? 0 : messageModel.Id.Value
            };
        }

        public static MessageModel MapMessageEntityToModel(Message message)
        {
            if (message == null) return null;
            return new MessageModel()
            {
                Author = new UserModel() { Id = message.UserId },
                Content = message.Content,
                CreationTime = message.CreationDate,
                Id = message.MessageId,
                isDeleted = (bool)message.IsDeleted

            };
        }


    }
}
