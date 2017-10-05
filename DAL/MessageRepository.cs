using DAL.Models;
using System;

namespace DAL
{
    public class MessageRepository
    {
        public void AddMessage(Message message)
        {
            using (var db = new ChatexdbContext())
            {
                db.Message.Add(message);
                db.SaveChanges();
            }
        }
    }
}
