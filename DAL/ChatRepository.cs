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
            using (var db = new ChatexdbContext())
            {
                db.Chat.Add(entity);
                db.SaveChanges();
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
            using (var db = new ChatexdbContext())
            {
                db.ChatUser.Add(entity);
                db.SaveChanges();
            }
        }

        public void AddUsersToChat(IEnumerable<ChatUserModel> chatUserModels)
        {
            using (var db = new ChatexdbContext())
            {
                var entities = chatUserModels.Select(x => ChatUserMapper.MapChatUserModelToEntity(x));
                db.ChatUser.AddRange(entities);
                db.SaveChanges();
            }
        }
    }
}
