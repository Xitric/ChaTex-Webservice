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
            using (var context = new ChatexdbContext())
            {
                return MessageMapper.MapMessageEntityToModel(
                    context.Message.Where(i => i.MessageId == messageId)
                    .Include(x => x.ChannelMessages)
                    .Include(x => x.User)
                    .ToList().FirstOrDefault());
            }
        }

        public IEnumerable<MessageModel> GetMessages(int channelId, DateTime before, int count)
        {
            using (var context = new ChatexdbContext())
            {
                return context.ChannelMessages
                    .Where(cm => cm.ChannelId == channelId)
                    .Include(cm => cm.Message)
                        .ThenInclude(m => m.User)
                    .ToList()
                    .Select(cm => cm.Message)
                    .OrderByDescending(m => m.CreationDate)
                    .SkipWhile(m => m.CreationDate >= before)
                    .Take(count)
                    .Reverse()
                    .ToList()
                    .Select(m => MessageMapper.MapMessageEntityToModel(m))
                    .ToList();
            }
        }

        private IEnumerable<Message> getMessageInChannel(int channelId)
        {
            using (var context = new ChatexdbContext())
            {
                return context.ChannelMessages
                    .Where(cm => cm.ChannelId == channelId)
                    .Include(cm => cm.Message)
                        .ThenInclude(m => m.User)
                    .ToList()
                    .Select(cm => cm.Message);
            }
        }

        public IEnumerable<MessageModel> GetMessagesSince(int channelId, DateTime since)
        {
            using (var context = new ChatexdbContext())
            {
                return getMessageInChannel(channelId)
                    .Where(m => m.CreationDate > since)
                    .OrderBy(m => m.CreationDate)
                    .Select(m => MessageMapper.MapMessageEntityToModel(m))
                    .ToList();
            }
        }

        public IEnumerable<MessageModel> GetDeletedMessagesSince(int channelId, DateTime since)
        {
            using (var context = new ChatexdbContext())
            {
                return getMessageInChannel(channelId)
                    .Where(m => m.DeletionDate > since)
                    .Select(m => MessageMapper.MapMessageEntityToModel(m))
                    .ToList();
            }
        }

        public IEnumerable<MessageModel> GetEditedMessagesSince(int channelId, DateTime since)
        {
            using (var context = new ChatexdbContext())
            {
                return getMessageInChannel(channelId)
                    .Where(m => m.LastEditDate > since)
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
                    Message messageEntity = MessageMapper.MapMessageModelToEntity(message);
                    messageEntity.CreationDate = DateTime.Now.ToUniversalTime();

                    context.Message.Add(messageEntity);
                    context.SaveChanges();

                    ChannelMessages channelMessageEntity = new ChannelMessages()
                    {
                        ChannelId = channelId,
                        MessageId = messageEntity.MessageId,
                    };

                    context.ChannelMessages.Add(channelMessageEntity);
                    context.SaveChanges();
                }
            }
        }

        public void DeleteMessage(int messageId)
        {
            using (var context = new ChatexdbContext())
            {
                Message entity = context.Message.Find(messageId);
                entity.DeletionDate = DateTime.Now.ToUniversalTime();

                context.SaveChanges();
            }
        }

        public void EditMessage(int messageId, string content)
        {
            using (var context = new ChatexdbContext())
            {
                Message entity = context.Message.Find(messageId);

                //Save message revision
                MessageRevision revision = new MessageRevision()
                {
                    Content = entity.Content,
                    EditDate = entity.LastEditDate == null ? entity.CreationDate : (DateTime)entity.LastEditDate,
                    MessageId = entity.MessageId
                };
                context.MessageRevision.Add(revision);

                //Update current message
                entity.Content = content;
                entity.LastEditDate = DateTime.Now.ToUniversalTime();

                context.SaveChanges();
            }
        }
    }
}
