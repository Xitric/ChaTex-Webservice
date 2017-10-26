using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Business;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Mapper;

namespace DAL
{
    class UserRepository : IUserRepository
    {
        public string GetSessionToken(string email)
        {
            using (var context = new ChatexdbContext())
            {
                int? userID = GetUserIdFromEmail(email);
                if (userID == null) return null;

                UserToken uToken = context.UserToken
                    .Find(userID);

                if (uToken != null)
                {
                    return uToken.Token;
                }

                return null;
            }
        }

        public bool SaveUserToken(string email, string token)
        {
            using (var context = new ChatexdbContext())
            {
                int? userID = GetUserIdFromEmail(email);
                if (userID == null) return false;

                UserToken uToken = new UserToken()
                {
                    UserId = (int)userID,
                    Token = token
                };

                context.UserToken.Add(uToken);
                context.SaveChanges();
            }

            return true;
        }

        public void DeleteUserToken(string email)
        {
            using (var context = new ChatexdbContext())
            {
                int? userID = GetUserIdFromEmail(email);
                if (userID == null) return;

                UserToken uToken = new UserToken()
                {
                    UserId = (int)userID
                };

                context.UserToken.Remove(uToken);
                context.SaveChanges();
            }
        }

        private int? GetUserIdFromEmail(string email)
        {
            using (var context = new ChatexdbContext())
            {
                try
                {
                    return context.User
                    .Where(u => u.Email.Equals(email))
                    .Select(u => u.UserId)
                    .Single();
                }
                catch (InvalidOperationException)
                {
                    return null;
                }
            }
        }

        public int? GetUserIdFromToken(string token)
        {
            using (var context = new ChatexdbContext())
            {
                try
                {
                    return context.UserToken
                    .Where(u => u.Token.Equals(token))
                    .Select(u => u.UserId)
                    .Single();
                }
                catch (InvalidOperationException)
                {
                    return null;
                }
            }
        }

        public IEnumerable<GroupModel> GetGroupsForUser(int userId)
        {
            using (var context = new ChatexdbContext())
            {
                IQueryable<Group> groupsUser = context.GroupUser
                    .Where(gu => gu.UserId == userId)
                    .Select(gu => gu.Group);

                //Select Group from GroupRole where the role id is in the collection:
                //  Select role id from UserRole where the user id is matched
                IQueryable<Group> groupsRole = context.GroupRole
                    .Where(gr => context.UserRole
                        .Where(ur => ur.UserId == userId)
                        .Select(ur => ur.Role.RoleId)
                        .Contains(gr.RoleId))
                    .Select(gr => gr.Group);

                return groupsUser.Union(groupsRole)
                    .Where(g => g.IsDeleted == false)
                    .Include(g => g.Channel)
                    .Select(g => GroupMapper.MapGroupEntityToModel(g))
                    .ToList();
            }
        }
    }
}
