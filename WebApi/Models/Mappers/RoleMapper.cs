using Business.Models;
using IO.Swagger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Mappers
{
    class RoleMapper
    {
        public static RoleDTO MapRoleToRoleDTO(RoleModel roleModel)
        {
            return new RoleDTO()
            {
                Id = roleModel.Id,
                Name = roleModel.Name,
                IsDeleted = roleModel.IsDeleted
            };
        }
    }
}
