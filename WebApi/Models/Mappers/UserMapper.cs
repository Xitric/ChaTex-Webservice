using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Mappers
{
    public class UserMapper
    {
        public static UserDTO MapUserToUserDTO(UserModel userModel, int callerId)
        {
            return new UserDTO()
            {
                Id = userModel.Id,
                FirstName = userModel.FirstName,
                MiddleInitial = userModel.MiddleInitial?.ToString(),
                LastName = userModel.LastName,
                Email = userModel.Email,
                Me = userModel.Id == callerId
            };
        }
    }
}
