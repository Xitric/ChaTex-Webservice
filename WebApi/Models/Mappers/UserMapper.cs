using Business.Models;
using IO.Swagger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Mappers
{
    class UserMapper
    {
        public static UserDTO MapUserToUserDTO(UserModel userModel, int callerId)
        {
            if (userModel == null) return null;
            return new UserDTO(userModel.Id, userModel.FirstName, userModel.MiddleInitial?.ToString(), userModel.LastName, userModel.Email, userModel.Id == callerId);
        }
    }
}
