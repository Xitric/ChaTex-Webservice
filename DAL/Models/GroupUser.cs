using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class GroupUser
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public bool? IsAdministrator { get; set; }

        public Group Group { get; set; }
        public User User { get; set; }
    }
}
