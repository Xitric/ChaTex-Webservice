using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class SystemAdministrator
    {
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
