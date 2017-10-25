using System;
using System.Collections.Generic;

namespace Business.Models
{
    public class GroupModel
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public UserModel Creator { get; set; }

        public DateTime? CreationTime { get; set; }

        public IEnumerable<ChannelModel> Channels { get; set; } = new List<ChannelModel>();

        public bool AllowEmployeeSticky { get; set; }

        public bool AllowEmployeeAcknowledgeable { get; set; }

        public bool AllowEmployeeBookmark { get; set; }
        public bool IsDeleted { get; set; }
    }
}
