using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Chats
{
    public interface IChatManager
    {
        void AddUsersToChat(int chatId, List<int> userIds);

        int? CreateChat(int userId, string chatName);

        IEnumerable<ChatModel> GetAllChatsForUser(int userId);
    }
}
