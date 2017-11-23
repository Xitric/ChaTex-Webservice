using Business;
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

                if (channel != null)
                {
                    channel.IsDeleted = true;
                    context.SaveChanges();
                }
            }
        }

        public ChannelModel GetChannel(int channelId)
        {
            using (var context = new ChatexdbContext())
            {
                return ChannelMapper.MapChannelEntityToModel(context.Channel.Where(i => i.ChannelId == channelId).FirstOrDefault());
            }
        }

        public void UpdateChannel(ChannelModel channelModel)
        {
            using (var context = new ChatexdbContext())
            {
                var channel = context.Channel.FirstOrDefault(c => c.ChannelId == channelModel.Id);

                if (channel != null)
                {
                    channel.Name = channelModel.Name;
                    context.SaveChanges();
                }
            }            
        }
    }
}
