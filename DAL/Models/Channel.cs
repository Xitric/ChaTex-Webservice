using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Channel
    {
        public Channel()
        {
            ChannelBookmark = new HashSet<ChannelBookmark>();
            ChannelMessages = new HashSet<ChannelMessages>();
        }

        public int ChannelId { get; set; }
        public string Name { get; set; }
        public bool? IsDeleted { get; set; }
        public int GroupId { get; set; }

        public Group Group { get; set; }
        public ICollection<ChannelBookmark> ChannelBookmark { get; set; }
        public ICollection<ChannelMessages> ChannelMessages { get; set; }
    }
}
