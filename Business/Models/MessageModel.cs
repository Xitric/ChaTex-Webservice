using System;

namespace Business.Models
{
    public class MessageModel
    {
        public int? Id { get; set; }

        private string content;
        public string Content {
            get => DeletionTime == null ? content : "";
            set => content = value;
        }

        public UserModel Author { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? DeletionTime { get; set; }

        public DateTime? LastEdited { get; set; }

        public int ChannelId { get; set; }
    }
}
