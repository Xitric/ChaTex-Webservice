using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public interface IChatRepository
    {
        int CreateChat(ChatModel chat);
        void AddUser(UserModel user, ChatModel chat);
        void AddUsersToChat(IEnumerable<ChatUserModel> chatUserModels);
    }
}
