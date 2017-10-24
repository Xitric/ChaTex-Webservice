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
            return new Message()
            {
                
            };
        }

        public static MessageModel MapMessageEntityToModel(Message message)
        {
            return new MessageModel()
            {

            };
        }


    }
}
