using System;
using System.Collections.Generic;
using System.Linq;
using Business;
using Business.Messages;
using Business.Models;
using DAL.Models;
using DAL.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    class MessageRepository : IDataAccess
    {
        private readonly IModelFactory modelFactory;
        private readonly ModelMapper modelMapper;

        public MessageRepository(IModelFactory modelFactory)
        {
            this.modelFactory = modelFactory;
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

                    return ToIMessage(dalMessage);
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
                    return ToIMessage(dalMessage);
                }
            }

            return null;
        }

        public List<IMessage> getMessagesSince(DateTime since)
        {
            using (var context = new ChatexdbContext())
            {
                return context.Message
                    .Where(msg => msg.CreationDate > since.ToUniversalTime())
                    .Include(msg => msg.User)
                    .Select(msg => ToIMessage(msg))
                    .ToList();
            }
        }

        //TODO: Move this somewhere else
        private IMessage ToIMessage(Message dalMessage)
        {
            User dalAuthor = dalMessage.User;

            IUser author = modelFactory.CreateUser(dalAuthor.UserId, dalAuthor.FirstName, dalAuthor.MiddleInitial?.ToCharArray()[0], dalAuthor.LastName, dalAuthor.Email);
            IMessage message = modelFactory.CreateMessage(dalMessage.MessageId, dalMessage.Content, author, dalMessage.CreationDate);

            return message;
        }
    }
}
