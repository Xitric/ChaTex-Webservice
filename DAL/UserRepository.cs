using DAL.Models;
using Business;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DAL.Mapper;

[assembly: InternalsVisibleTo("ChaTexTest")]
namespace DAL
{
    class UserRepository : IUserRepository
    {
        public string GetSessionToken(string email)
        {
            using (var context = new ChatexdbContext())
            {
                int? userID = GetUserIdFromEmail(email);

                if (userID == null)
                {
                    return null;
                }

                UserToken userToken = context.UserToken
                    .Find(userID);

                if (userToken != null)
                {
                    return userToken.Token;
                }

                return null;
            }
        }

        public IEnumerable<UserModel> GetAllUsers()
        {
            using (var context = new ChatexdbContext())
            {
                return context.User.Select(u => UserMapper.MapUserEntityToModel(u)).ToList();
            }
        }

        public bool SaveUserToken(string email, string token)
        {
            using (var context = new ChatexdbContext())
            {
                int? userID = GetUserIdFromEmail(email);

                if (userID == null)
                {
                    return false;
                }

                UserToken userToken = new UserToken()
                {
                    UserId = (int)userID,
                    Token = token
                };

                context.UserToken.Add(userToken);
                context.SaveChanges();
            }

            return true;
        }

        public void DeleteUserToken(string email)
        {
            using (var context = new ChatexdbContext())
            {
                int? userID = GetUserIdFromEmail(email);

                if (userID == null)
                {
                    return;
                }

                UserToken userToken = new UserToken()
                {
                    UserId = (int)userID
                };

                context.UserToken.Remove(userToken);
                context.SaveChanges();
            }
        }

        private int? GetUserIdFromEmail(string email)
        {
            using (var context = new ChatexdbContext())
            {
                var matchingUserIds = context.User
                    .Where(u => u.Email.Equals(email))
                    .Select(u => u.UserId);

                if (matchingUserIds.Count() == 1)
                {
                    return matchingUserIds.Single();
                }
                else
                {
                    return null;
                }
            }
        }

        public int? GetUserIdFromToken(string token)
        {
            using (var context = new ChatexdbContext())
            {
                return context.UserToken
                .Where(u => u.Token.Equals(token))
                .Select(u => u.UserId)
                .FirstOrDefault();
            }
        }

        public void UpdateUser(UserModel userModel)
        {
            using (var context = new ChatexdbContext())
            {
                User userEntity = context.User.Where(id => id.UserId == userModel.Id).FirstOrDefault();

                userEntity.FirstName = userModel.FirstName;
                userEntity.MiddleInitial = userModel.MiddleInitial.ToString();
                userEntity.LastName = userModel.LastName;
                userEntity.Email = userModel.Email;
                userEntity.IsDeleted = userModel.IsDeleted;

                context.SaveChanges();
            }
        }

        public UserModel GetUser(int userId)
        {
            using (var context = new ChatexdbContext())
            {
                var user = context.User.Where(i => i.UserId == userId).FirstOrDefault();

                return UserMapper.MapUserEntityToModel(user);
            }
        }

        public bool IsUserAdmin(int userId)
        {
            using (var context = new ChatexdbContext())
            {
                return context.SystemAdministrator.Where(x => x.UserId == userId).Any();
            }
        }
        public IEnumerable<RoleModel> GetAllUserRoles(int userId)
        {
            using (var context = new ChatexdbContext())
            {
                List<RoleModel> userRoles = context.UserRole
                .Where(ur => ur.UserId == userId)
                .Select(ur => RoleMapper.MapRoleEntityToModel(ur.Role))
                .ToList();
                return userRoles;
            }
        }
    }
}