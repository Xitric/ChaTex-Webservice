﻿using Business;
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

        public void DeleteChannel(int groupId, int channelId)
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
    }
}