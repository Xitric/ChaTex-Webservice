using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Chats
{
    public interface IChatManager
    {
        void AddUsersToChat(int chatId, List<int> userIds);

        /// <summary>
        /// Creates a chat between two useres.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="chatName"></param>
        /// <returns></returns>

        int? CreateChat(int userId, string chatName);

        IEnumerable<ChatModel> GetAllChatsForUser(int userId);
    }
}
