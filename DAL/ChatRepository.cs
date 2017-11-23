using System;
using Business;
using Business.Models;
using DAL.Models;
using DAL.Mapper;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    class ChatRepository : IChatRepository
    {
        public int CreateChat(ChatModel chat)
        {
            var entity = ChatMapper.MapChatModelToEntity(chat);

            using (var context = new ChatexdbContext())
            {
                context.Chat.Add(entity);
                context.SaveChanges();
            }

            return entity.ChatId;
        }
        public void AddUser(UserModel user, ChatModel chat)
        {
            var entity = new ChatUser()
            {
                ChatId = (int)chat.Id,
                UserId = (int)user.Id
            };

            using (var context = new ChatexdbContext())
            {
                context.ChatUser.Add(entity);
                context.SaveChanges();
            }
        }

        public void AddUsersToChat(IEnumerable<ChatUserModel> chatUserModels)
        {
            using (var context = new ChatexdbContext())
            {
                var entities = chatUserModels.Select(x => ChatUserMapper.MapChatUserModelToEntity(x));

                context.ChatUser.AddRange(entities);
                context.SaveChanges();
            }
        }
    }
}
