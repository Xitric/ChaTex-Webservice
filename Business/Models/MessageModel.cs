using System;

namespace Business.Models
{
    public class MessageModel
    {
        public int? Id { get; set; }

        public string Content { get; set; }

        public UserModel Author { get; set; }

        public DateTime? CreationTime { get; set; }
    }
}
