using Business.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Mapper
{
    class ChatUserMapper
    {
        public static ChatUser MapChatUserModelToEntity(ChatUserModel chatUserModel)
        {
            if (chatUserModel == null)
            {
                return null;
            }

            return new ChatUser()
            {
                ChatId = chatUserModel.Chat.Id == null ? 0 : (int)chatUserModel.Chat.Id,
                UserId = chatUserModel.User.Id == null ? 0 : (int)chatUserModel.User.Id
            };
        }

        public static ChatUserModel MapChatUserEntityToModel(ChatUser chatUser)
        {
            if (chatUser == null)
            {
                return null;
            }

            return new ChatUserModel()
            {
                Chat = ChatMapper.MapChatEntityToModel(chatUser.Chat),
                User = UserMapper.MapUserEntityToModel(chatUser.User),
            };
        }
    }
}
