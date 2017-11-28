using System;
using System.Collections.Generic;
using System.Text;
using Business.Models;
using System.Linq;

namespace Business.Chats
{
    class ChatManager : IChatManager
    {
        private readonly IChatRepository chatRepository;

        public ChatManager(IChatRepository chatRepository)
        {
            this.chatRepository = chatRepository;
        }

        public void AddUsersToChat(int chatId, List<int> userIds)
        {            
            chatRepository.AddUsersToChat(chatUserModels: userIds.Select(userId => new ChatUserModel()
            {
                Chat = new ChatModel()
                {
                    Id = chatId
                },
                User = new UserModel()
                {
                    Id = userId
                }
            }));
        }

        public IEnumerable<ChatModel> GetAllChatsForUser(int userId)
        {
            throw new NotImplementedException();
        }

        public int? CreateChat(int userId, string chatName)
        {
            var user = new UserModel()
            {
                Id = userId
            };

            ChatModel chat = new ChatModel()
            {
                Name = chatName
            };

            chat.Id = chatRepository.CreateChat(chat);
            chatRepository.AddUser(user, chat);

            return chat.Id;
        }
    }
}
