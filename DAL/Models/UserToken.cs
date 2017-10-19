using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class UserToken
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }

        public User User { get; set; }
    }
}
