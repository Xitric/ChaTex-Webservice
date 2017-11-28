using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class GroupMembershipDetails
    {
        public int UserId { get; set; }

        public int GroupId { get; set; }

        public bool IsMember { get; set; }

        public bool IsAdministrator { get; set; }
    }
}
