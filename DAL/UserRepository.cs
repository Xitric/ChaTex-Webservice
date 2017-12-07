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
        public string GetSessionToken(int userId)
        {
            using (var context = new ChatexdbContext())
            {
                return context.UserToken
                    .Find(userId)?
                    .Token;
            }
        }

        public void SaveUserToken(int userId, string token)
        {
            using (var context = new ChatexdbContext())
            {
                UserToken userToken = new UserToken()
                {
                    UserId = userId,
                    Token = token
                };

                context.UserToken.Add(userToken);
                context.SaveChanges();
            }
        }

        public void DeleteUserToken(int userId)
        {
            using (var context = new ChatexdbContext())
            {
                UserToken userToken = new UserToken()
                {
                    UserId = userId
                };

                context.UserToken.Remove(userToken);
                context.SaveChanges();
            }
        }

        public int? GetUserIdFromEmail(string email)
        {
            using (var context = new ChatexdbContext())
            {
                return context.User
                    .Where(u => u.Email.Equals(email))
                    .Select(u => (int?)u.UserId)
                    .SingleOrDefault();
            }
        }

        public string GetUserPasswordHash(int userId)
        {
            using (var context = new ChatexdbContext())
            {
                return context.User
                    .Find(userId)?
                    .PasswordHash;
            }
        }

        public byte[] GetUserSalt(int userId)
        {
            using (var context = new ChatexdbContext())
            {
                return context.User
                    .Find(userId)?
                    .Salt;
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

        public IEnumerable<UserModel> GetAllUsers()
        {
            using (var context = new ChatexdbContext())
            {
                return context.User.Select(u => UserMapper.MapUserEntityToModel(u)).ToList();
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