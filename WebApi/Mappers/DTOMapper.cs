using Business.Models;
using System;
using WebAPI.Models;

namespace WebAPI.Mappers
{
    class DTOMapper
    {
        public GetMessage ConvertMessage(IMessage message)
        {
            return new GetMessage(message.Id, message.CreationTime, message.Content, ConvertUser(message.Author));
        }

        public User ConvertUser(IUser user)
        {
            return new User(user.Id, user.FirstName, user.MiddleInitial?.ToString(), user.LastName, user.Email);
        }
    }
}
