using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Role
    {
        public Role()
        {
            GroupRole = new HashSet<GroupRole>();
            UserRole = new HashSet<UserRole>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool? IsDeleted { get; set; }

        public ICollection<GroupRole> GroupRole { get; set; }
        public ICollection<UserRole> UserRole { get; set; }
    }
}
