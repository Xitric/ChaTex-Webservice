using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Group
    {
        public Group()
        {
            Channel = new HashSet<Channel>();
            GroupRole = new HashSet<GroupRole>();
            GroupUser = new HashSet<GroupUser>();
        }

        public int GroupId { get; set; }
        public string Name { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool AllowEmployeeSticky { get; set; }
        public bool AllowEmployeeAcknowledgeable { get; set; }
        public bool AllowEmployeeBookmark { get; set; }

        public User CreatedByNavigation { get; set; }
        public ICollection<Channel> Channel { get; set; }
        public ICollection<GroupRole> GroupRole { get; set; }
        public ICollection<GroupUser> GroupUser { get; set; }
    }
}
