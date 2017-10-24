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

            };
        }

        public static UserModel MapUserEntityToModel(User user)
        {
            return new UserModel()
            {

            };
        }
    }
}
