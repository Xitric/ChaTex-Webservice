using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class User
    {
        public User()
        {
            ChatUser = new HashSet<ChatUser>();
            Group = new HashSet<Group>();
            GroupUser = new HashSet<GroupUser>();
            Message = new HashSet<Message>();
            UserRole = new HashSet<UserRole>();
            UserSavedMessage = new HashSet<UserSavedMessage>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddelInitial { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? IsDeleted { get; set; }

        public SystemAdministrator SystemAdministrator { get; set; }
        public UserToken UserToken { get; set; }
        public ICollection<ChatUser> ChatUser { get; set; }
        public ICollection<Group> Group { get; set; }
        public ICollection<GroupUser> GroupUser { get; set; }
        public ICollection<Message> Message { get; set; }
        public ICollection<UserRole> UserRole { get; set; }
        public ICollection<UserSavedMessage> UserSavedMessage { get; set; }
    }
}
