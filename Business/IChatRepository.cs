using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public interface IChatRepository
    {
        int CreateChat(ChatModel chatmodel);
        void AddUser(UserModel usermodel, ChatModel chatmodel);
        void AddUsersToChat(IEnumerable<ChatUserModel> chatUserModels);
    }
}
