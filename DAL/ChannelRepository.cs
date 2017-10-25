using Business;
using DAL.Models;

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
    }
}
