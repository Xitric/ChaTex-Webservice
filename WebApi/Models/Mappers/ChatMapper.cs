using Business.Models;
using IO.Swagger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Mappers
{
    public class ChatMapper
    {
        public static ChatDTO MapChatToChatDTO (ChatModel chat)
        {
            // List<UserDTO> users = chat.Users
            // .Select(c => UserMapper.MapUserToUserDTO(c))
            //.ToList();
            return null;
         //   return new ChatDTO(chat.Id, chat.Name, users);   
        }
    }
}
