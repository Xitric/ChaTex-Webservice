using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class ChatUserModel
    {
        public ChatModel Chat { get; set; }
        public UserModel User { get; set; }
    }
}
