using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class GroupRole
    {
        public int GroupId { get; set; }
        public int RoleId { get; set; }

        public Group Group { get; set; }
        public Role Role { get; set; }
    }
}
