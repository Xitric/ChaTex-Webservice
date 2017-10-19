using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public Role Role { get; set; }
        public User User { get; set; }
    }
}
