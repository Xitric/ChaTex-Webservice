using Business.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Mapper
{
    class UserMapper
    {
        public static User MapUserModelToEntity(UserModel userModel)
        {
            return new User()
            {
                Email = userModel.Email,
                IsDeleted = userModel.IsDeleted,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                MiddleInitial = userModel.MiddleInitial?.ToString(),
                UserId = userModel.Id == null ? 0 : (int)userModel.Id

            };
        }

        public static UserModel MapUserEntityToModel(User user)
        {
            return new UserModel()
            {
                Email = user.Email,
                Id = user.UserId,
                FirstName = user.FirstName,
                IsDeleted = user.IsDeleted == null ? false : (bool)user.IsDeleted,
                LastName = user.LastName,
                MiddleInitial = user.MiddleInitial?[0]
            };
        }
    }
}
