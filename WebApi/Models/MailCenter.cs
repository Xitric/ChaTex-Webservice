using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class MailCenter : IMailCenter
    {
        private long NextID;
        private List<Message> messages;

        public MailCenter()
        {
            NextID = 0;
            messages = new List<Message>();
        }

        public void RegisterMessage(Message message)
        {
            message.Id = NextID++;
            message.CreationTime = DateTime.Now;
            messages.Add(message);
        }

        public IEnumerable<Message> GetMessages()
        {
            return messages;
        }

        public Message GetMessage(long id)
        {
            return messages.FirstOrDefault(m => m.Id == id);
        }

        public IEnumerable<Message> GetMessages(DateTime since)
        {
            return messages.Where(m => m.CreationTime > since).ToList();
        }
    }
}
