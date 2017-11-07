﻿using Business;
using Business.Models;
using DAL.Mapper;
using DAL.Models;
using System.Linq;

namespace DAL
{
    class ChannelRepository : IChannelRepository
    {
        public void CreateChannel(int groupId, string name)
        {
            using (var db = new ChatexdbContext())
            {
                Channel channel = new Channel()
                {
                    GroupId = groupId,
                    Name = name
                };

                db.Channel.Add(channel);
                db.SaveChanges();
            }
        }

        public void DeleteChannel(int channelId)
        {
            using (var db = new ChatexdbContext())
            {
                var channel = db.Channel.FirstOrDefault(c => c.ChannelId == channelId);
                if(channel != null)
                {
                    channel.IsDeleted = true;
                    db.SaveChanges();
                }
            }
        }

        public ChannelModel GetChannel(int channelId)
        {
            using (var db = new ChatexdbContext())
            {
                return ChannelMapper.MapChannelEntityToModel(db.Channel.Where(i => i.ChannelId == channelId).FirstOrDefault());
            }
        }

        public void UpdateChannel(ChannelModel channelModel)
        {
            using (var db = new ChatexdbContext())
            {
                var channel = db.Channel.FirstOrDefault(c => c.ChannelId == channelModel.Id);
                if(channel != null)
                {
                    channel.Name = channelModel.Name;
                    db.SaveChanges();
                }
            }            
        }
    }
}
