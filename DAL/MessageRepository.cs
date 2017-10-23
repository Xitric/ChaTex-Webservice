using System;
using System.Collections.Generic;
using System.Linq;
using Business;
using Business.Models;
using DAL.Models;
using DAL.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    class MessageRepository : IMessageRepository
    {
        private readonly ModelMapper modelMapper;

        public MessageRepository()
        {
            modelMapper = new ModelMapper();
        }

        public IMessage AddMessage(IMessage message)
        {
            if (message != null)
            {
                using (var context = new ChatexdbContext())
                {
                    Message dalMessage = modelMapper.ConvertMessage(message);

                    //Overwrite the CreationDate property when adding new messages
                    dalMessage.CreationDate = DateTime.Now;

                    context.Message.Add(dalMessage);
                    context.SaveChanges();

                    //Load author information for returning the newly created message
                    context.Entry(dalMessage).Reference(msg => msg.User).Load();

                    return new MessageMapper(dalMessage);
                }
            }

            return null;
        }

        public IMessage GetMessage(long id)
        {
            using (var context = new ChatexdbContext())
            {
                Message dalMessage = context.Message
                    .Where(msg => msg.MessageId == (int)id)
                    .Include(msg => msg.User)
                    .FirstOrDefault();

                if (dalMessage != null)
                {
                    return new MessageMapper(dalMessage);
                }
            }

            return null;
        }

        public List<IMessage> GetMessagesSince(DateTime since)
        {
            using (var context = new ChatexdbContext())
            {
                return context.Message
                    .Where(msg => msg.CreationDate > since.ToUniversalTime())
                    .Include(msg => msg.User)
                    .Select(msg => new MessageMapper(msg))
                    .ToList<IMessage>();
            }
        }
    }
}
