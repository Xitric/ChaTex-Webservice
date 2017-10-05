using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public interface IMailCenter
    {
        void RegisterMessage(Message message);
        IEnumerable<Message> GetMessages();
        Message GetMessage(long id);
        IEnumerable<Message> GetMessages(DateTime since);
    }
}
