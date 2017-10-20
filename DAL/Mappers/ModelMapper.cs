using Business.Models;
using DAL.Models;
using System;

namespace DAL.Mappers
{
    class ModelMapper
    {
        public Message ConvertMessage(IMessage message)
        {
            return new Message()
            {
                MessageId = message.Id == null ? default(int) : (int)message.Id,
                Content = message.Content,
                EditDate = null, //TODO: Not implemented
                UserId = message.Author.Id == null ? default(int) : (int)message.Author.Id,
                IsDeleted = false, //TODO: Not implemented
                CreationDate = message.CreationTime == null ? default(DateTime) : (DateTime)message.CreationTime
            };
        }

        public User ConvertUser(IUser user)
        {
            return new User()
            {
                UserId = user.Id == null ? default(int) : (int)user.Id,
                FirstName = user.FirstName,
                MiddleInitial = user.MiddleInitial?.ToString(),
                LastName = user.LastName,
                Email = user.Email,
                IsDeleted = false //TODO: Not implemented
            };
        }
    }
}