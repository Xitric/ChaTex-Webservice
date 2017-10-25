using Business.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Mapper
{
    class MessageMapper
    {
        public static Message MapMessageModelToEntity(MessageModel messageModel)
        {
            if (messageModel == null) return null;
            return new Message()
            {
                
            };
        }

        public static MessageModel MapMessageEntityToModel(Message message)
        {
            if (message == null) return null;
            return new MessageModel()
            {

            };
        }


    }
}
