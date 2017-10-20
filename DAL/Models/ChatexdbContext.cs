using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DAL.Models
{
    public partial class ChatexdbContext : DbContext
    {
        public virtual DbSet<Channel> Channel { get; set; }
        public virtual DbSet<ChannelBookmark> ChannelBookmark { get; set; }
        public virtual DbSet<ChannelMessages> ChannelMessages { get; set; }
        public virtual DbSet<Chat> Chat { get; set; }
        public virtual DbSet<ChatMessage> ChatMessage { get; set; }
        public virtual DbSet<ChatUser> ChatUser { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<GroupRole> GroupRole { get; set; }
        public virtual DbSet<GroupUser> GroupUser { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<SystemAdministrator> SystemAdministrator { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<UserSavedMessage> UserSavedMessage { get; set; }
        public virtual DbSet<UserToken> UserToken { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=chatexdb.database.windows.net;Database=Chatexdb;Trusted_Connection=False;User ID=chatexusername;Password=Chatexpassword123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Channel>(entity =>
            {
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Channel)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Channel_Group");
            });

            modelBuilder.Entity<ChannelBookmark>(entity =>
            {
                entity.HasKey(e => new { e.ChannelId, e.MessageId });

                entity.HasOne(d => d.Channel)
                    .WithMany(p => p.ChannelBookmark)
                    .HasForeignKey(d => d.ChannelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChannelBookmark_Channel");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.ChannelBookmark)
                    .HasForeignKey(d => d.MessageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChannelBookmark_Message");
            });

            modelBuilder.Entity<ChannelMessages>(entity =>
            {
                entity.HasKey(e => new { e.MessageId, e.ChannelId });

                entity.HasOne(d => d.Channel)
                    .WithMany(p => p.ChannelMessages)
                    .HasForeignKey(d => d.ChannelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChannelMessages_Channel");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.ChannelMessages)
                    .HasForeignKey(d => d.MessageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChannelMessages_Message");
            });

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasKey(e => new { e.ChatId, e.MessageId });

                entity.HasOne(d => d.Chat)
                    .WithMany(p => p.ChatMessage)
                    .HasForeignKey(d => d.ChatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatMessage_Chat");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.ChatMessage)
                    .HasForeignKey(d => d.MessageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatMessage_Message");
            });

            modelBuilder.Entity<ChatUser>(entity =>
            {
                entity.HasKey(e => new { e.ChatId, e.UserId });

                entity.HasOne(d => d.Chat)
                    .WithMany(p => p.ChatUser)
                    .HasForeignKey(d => d.ChatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatUser_Chat");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ChatUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatUser_User");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Group)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Group_User");
            });

            modelBuilder.Entity<GroupRole>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.RoleId });

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupRole)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupRole_Group");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.GroupRole)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupRole_Role");
            });

            modelBuilder.Entity<GroupUser>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.UserId });

                entity.Property(e => e.IsAdministrator).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupUser)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupUser_Group");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GroupUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupUser_User");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Message_User");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RoleName).IsRequired();
            });

            modelBuilder.Entity<SystemAdministrator>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.HasOne(d => d.User)
                    .WithOne(p => p.SystemAdministrator)
                    .HasForeignKey<SystemAdministrator>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SystemAdministrator_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.FirstName).IsRequired();

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastName).IsRequired();

                entity.Property(e => e.MiddleInitial).HasColumnType("char(4)");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_Role");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_User");
            });

            modelBuilder.Entity<UserSavedMessage>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.MessageId });

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.UserSavedMessage)
                    .HasForeignKey(d => d.MessageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSavedMessage_Message");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSavedMessage)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSavedMessage_User");
            });

            modelBuilder.Entity<UserToken>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.ExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserToken)
                    .HasForeignKey<UserToken>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserToken_User");
            });
        }
    }
}
