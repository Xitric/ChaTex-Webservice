using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Business;
using Business.Models;
using DAL.Mapper;

namespace DAL
{
    class MessageRepository : IMessageRepository
    {
        public void AddMessage(MessageModel message)
        {
            if (message != null)
            {
                using (var context = new ChatexdbContext())
                {
                    Message dalMessage = MessageMapper.MapMessageModelToEntity(message);

                    //Overwrite the CreationDate property when adding new messages
                    dalMessage.CreationDate = DateTime.Now;

                    context.Message.Add(dalMessage);
                    context.SaveChanges();

                    //Load author information for returning the newly created message
                    context.Entry(dalMessage).Reference(msg => msg.User).Load();
                }
            }
        }

        public MessageModel GetMessage(long id)
        {
            using (var context = new ChatexdbContext())
            {
                Message dalMessage = context.Message
                    .Where(msg => msg.MessageId == (int)id)
                    .Include(msg => msg.User)
                    .FirstOrDefault();

                if (dalMessage != null)
                {
                    return MessageMapper.MapMessageEntityToModel(dalMessage);
                }
            }

            return null;
        }

        public List<MessageModel> GetMessagesSince(DateTime since)
        {
            using (var context = new ChatexdbContext())
            {
                return context.Message
                    .Where(msg => msg.CreationDate > since.ToUniversalTime())
                    .Include(msg => msg.User)
                    .Select(msg => MessageMapper.MapMessageEntityToModel(msg))
                    .ToList();
            }
        }
    }
}
