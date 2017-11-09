using Business.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Mapper
{
    class ChatMapper
    {
        public static Chat MapChatModelToEntity(ChatModel chatModel)
        {
            if (chatModel == null) return null;
            return new Chat()
            {
                ChatId = chatModel.Id == null ? 0 : chatModel.Id.Value,
                Name = chatModel.Name
            };
        }
        public static ChatModel MapChatEntityToModel(Chat chat)
        {
            if (chat == null) return null;
            return new ChatModel()
            {  
                Id = chat.ChatId,
                Name = chat.Name
            };
        }
    }
}
