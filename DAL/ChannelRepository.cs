using Business;
using Business.Models;
using DAL.Mapper;
using DAL.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    class ChannelRepository : IChannelRepository
    {
        private enum ChannelEventType : byte
        {
            ChannelDelete,
            ChannelEdit
        }

        public void CreateChannel(int groupId, string name)
        {
            using (var context = new ChatexdbContext())
            {
                Channel channel = new Channel()
                {
                    GroupId = groupId,
                    Name = name
                };

                context.Channel.Add(channel);
                context.SaveChanges();
            }
        }

        public void DeleteChannel(int channelId)
        {
            using (var context = new ChatexdbContext())
            {
                var channel = context.Channel.FirstOrDefault(c => c.ChannelId == channelId);
                if (channel == null) return;

                channel.IsDeleted = true;

                context.ChannelEvent.Add(new ChannelEvent()
                {
                    ChannelId = channel.ChannelId,
                    TimeOfOccurrence = DateTime.UtcNow,
                    EventType = (byte)ChannelEventType.ChannelDelete
                });

                context.SaveChanges();
            }
        }

        public ChannelModel GetChannel(int channelId)
        {
            using (var context = new ChatexdbContext())
            {
                return ChannelMapper.MapChannelEntityToModel(context.Channel.Where(c => c.ChannelId == channelId).Where(c => c.IsDeleted == false).FirstOrDefault());
            }
        }

        public void UpdateChannel(ChannelModel channelModel)
        {
            using (var context = new ChatexdbContext())
            {
                var channel = context.Channel.FirstOrDefault(c => c.ChannelId == channelModel.Id);
                if (channel == null) return;

                channel.Name = channelModel.Name;

                context.ChannelEvent.Add(new ChannelEvent()
                {
                    ChannelId = channel.ChannelId,
                    TimeOfOccurrence = DateTime.UtcNow,
                    EventType = (byte)ChannelEventType.ChannelEdit
                });

                context.SaveChanges();
            }
        }

        public DateTime? GetChannelDeletionDate(int channelId)
        {
            using (var context = new ChatexdbContext())
            {
                return context.ChannelEvent
                    .Where(ce => ce.ChannelId == channelId)
                    .Where(ce => ce.EventType == (byte)ChannelEventType.ChannelDelete)
                    .Max(ce => ce.TimeOfOccurrence);
            }
        }

        public DateTime? GetChannelRenameDate(int channelId)
        {
            using (var context = new ChatexdbContext())
            {
                return context.ChannelEvent
                    .Where(ce => ce.ChannelId == channelId)
                    .Where(ce => ce.EventType == (byte)ChannelEventType.ChannelEdit)
                    .Max(ce => ce.TimeOfOccurrence);
            }
        }

        public IEnumerable<ChannelModel> GetChannelDeletionsSince(DateTime since)
        {
            return getChannelsByEventSince(since, ChannelEventType.ChannelDelete);
        }

        public IEnumerable<ChannelModel> GetChannelRenamesSince(DateTime since)
        {
            return getChannelsByEventSince(since, ChannelEventType.ChannelEdit);
        }

        private IEnumerable<ChannelModel> getChannelsByEventSince(DateTime since, ChannelEventType eventType)
        {
            using (var context = new ChatexdbContext())
            {
                return context.ChannelEvent
                    .Where(ce => ce.EventType == (byte)eventType)
                    .Where(ce => ce.TimeOfOccurrence > since)
                    .Include(ce => ce.Channel)
                    .ToList()
                    .Select(ce => ChannelMapper.MapChannelEntityToModel(ce.Channel));
            }
        }
    }
}
