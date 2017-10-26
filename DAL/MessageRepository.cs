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
                return db.ChannelMessages
                    .Where(cm => cm.ChannelId == channelId)
                    .Select(cm => cm.Message)
                    .Where(m => m.IsDeleted == false)
                    .OrderBy(m => m.CreationDate)
                    .Skip(from)
                    .Take(count)
                    .Select(m => MessageMapper.MapMessageEntityToModel(m));
            }
        }
    }
}
