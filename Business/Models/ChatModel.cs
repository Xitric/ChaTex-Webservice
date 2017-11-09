using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class ChatModel
    {
        public int? Id { get; set; }

        public string Name { get; set; }

       public IEnumerable<UserModel> Users { get; set; } = new List<UserModel>();
    }
}
