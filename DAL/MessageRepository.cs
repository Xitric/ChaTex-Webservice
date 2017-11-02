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
        public MessageModel GetMessage(int messageId)
        {
            using(var db = new ChatexdbContext())
            {
                return MessageMapper.MapMessageEntityToModel(db.Message.Where(i => i.MessageId == messageId).Include(x => x.ChannelMessages).Include(x => x.User).ToList().FirstOrDefault());
            }
        }

        public IEnumerable<MessageModel> GetMessages(int channelId, int from, int count)
        {
            using (var db = new ChatexdbContext())
            {
                return db.ChannelMessages
                    .Where(cm => cm.ChannelId == channelId)
                    .Include(cm => cm.Message)
                        .ThenInclude(m => m.User)
                    .ToList()
                    .Select(cm => cm.Message)
                    .Where(m => m.IsDeleted == false)
                    .OrderByDescending(m => m.CreationDate)
                    .Skip(from)
                    .Take(count)
                    .Reverse()
                    .ToList()
                    .Select(m => MessageMapper.MapMessageEntityToModel(m))
                    .ToList();
            }
        }

        public IEnumerable<MessageModel> getMessagesSince(int channelId, DateTime since)
        {
            using (var db = new ChatexdbContext())
            {
                Console.WriteLine(since);
                return db.ChannelMessages
                    .Where(cm => cm.ChannelId == channelId)
                    .Include(cm => cm.Message)
                        .ThenInclude(m => m.User)
                    .ToList()
                    .Select(cm => cm.Message)
                    .Where(m => m.IsDeleted == false)
                    .Where(m => m.CreationDate > since)
                    .OrderBy(m => m.CreationDate)
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

        public void DeleteMessage(int messageId)
        {
            using (var context = new ChatexdbContext())
            {
                Message dalmessage = context.Message.Where(i => i.MessageId == messageId).FirstOrDefault();
                dalmessage.IsDeleted = true;
                context.SaveChanges();
            }
        }

    }
}
