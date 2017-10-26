using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Business;
using Business.Models;
using DAL.Mapper;

namespace DAL
{
    class MessageRepository : IMessageRepository
    {
        public IEnumerable<MessageModel> getMessages(int channelId, int from, int count)
        {
            using (var db = new ChatexdbContext())
            {
                //var messages = db.ChannelMessages
                //    .Where(cm => cm.ChannelId == channelId)
                //    .Include(cm => cm.Message)
                //    .ThenInclude(m => m.User)
                //    .ToList()
                //    .Select(cm => cm.Message)
                //    .Select(cm => MessageMapper.MapMessageEntityToModel(cm))
                //    .ToList();


                /*var messages = db.ChannelMessages
                    .Where(cm => cm.ChannelId == channelId)
                    .Include(cm => cm.Message)
                    .ThenInclude(m => m.User)
                    .Select(cm => cm.Message)
                    .ToList()
                    .Select(cm => MessageMapper.MapMessageEntityToModel(cm))
                    .ToList();
                    */
                //return messages;


                return db.ChannelMessages
                    .Where(cm => cm.ChannelId == channelId)
                    .Include(cm => cm.Message)
                        .ThenInclude(m => m.User)
                    .ToList()
                    .Select(cm => cm.Message)
                    .Where(m => m.IsDeleted == false)
                    .OrderBy(m => m.CreationDate)
                    .Skip(from)
                    .Take(count)
                    .ToList()
                    .Select(m => MessageMapper.MapMessageEntityToModel(m))
                    .ToList();
            }
        }

        public void CreateMessage(MessageModel message, int channelId)
        {
            if (message != null)
            {
                using (var context = new ChatexdbContext())
                {
                    Message dalmessage = MessageMapper.MapMessageModelToEntity(message);
                    dalmessage.CreationDate = DateTime.Now;

                    context.Message.Add(dalmessage);
                    context.SaveChanges();

                    ChannelMessages channelMessage = new ChannelMessages()
                    {
                        ChannelId = channelId,
                        MessageId = dalmessage.MessageId,
                    };
                    context.ChannelMessages.Add(channelMessage);
                    context.SaveChanges();
                }
            }
        }

    }
}
