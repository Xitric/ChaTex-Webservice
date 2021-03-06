/*
 * ChaTex Web API
 *
 * The Web API for ChaTex
 *
 * OpenAPI spec version: v1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using IO.Swagger.Attributes;
using IO.Swagger.Models;
using WebAPI.Authentication;
using Business.Users;
using Business.Groups;
using Business.Models;
using System.Linq;
using WebAPI.Models.Mappers;
using Business.Errors;

namespace IO.Swagger.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class UsersApiController : Controller
    {
        private readonly IUserManager userManager;
        private readonly IGroupManager groupManager;

        public UsersApiController(IUserManager userManager, IGroupManager groupManager)
        {
            this.userManager = userManager;
            this.groupManager = groupManager;
        }

        /// <summary>
        /// Get a list of all users registered in the system
        /// </summary>

        /// <response code="200">Successfully retrieved all the users</response>
        [HttpGet]
        [Route("/1.0.0/users")]
        [ValidateModelState]
        [SwaggerOperation("UsersGetAllUsers")]
        [SwaggerResponse(200, typeof(List<UserDTO>), "Successfully retrieved all the users")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult UsersGetAllUsers()
        {
            int callerId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            IEnumerable<UserModel> users = userManager.GetAllUsers();
            IEnumerable<UserDTO> dtoResponse = users.Select(u => UserMapper.MapUserToUserDTO(u, callerId));

            return new ObjectResult(dtoResponse);
        }

        /// <summary>
        /// Get the the groups that a user is a member of
        /// </summary>

        /// <response code="200">Successfully retrieved the user&#39;s groups</response>
        [HttpGet]
        [Route("/1.0.0/users/me/groups")]
        [ValidateModelState]
        [SwaggerOperation("UsersGetGroupsForUser")]
        [SwaggerResponse(200, typeof(List<GroupDTO>), "Successfully retrieved the user&#39;s groups")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult UsersGetGroupsForUser()
        {
            int callerId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            IEnumerable<GroupModel> groups = groupManager.GetGroupsForUser(callerId);
            List<GroupDTO> dtoResponse = groups.Select(g => GroupMapper.MapGroupToGroupDTO(g)).ToList();

            return new ObjectResult(dtoResponse);
        }

        /// <summary>
        /// Sign into the system
        /// </summary>

        /// <param name="userCredentials">The user&#39;s email and password</param>
        /// <response code="200">The user was successfully logged in and an authentication token was generated</response>
        [HttpPost]
        [Route("/1.0.0/users/login")]
        [ValidateModelState]
        [SwaggerOperation("UsersLogin")]
        [SwaggerResponse(200, typeof(string), "The user was successfully logged in and an authentication token was generated")]
        public virtual IActionResult UsersLogin([FromBody]UserCredentials userCredentials)
        {
            string token = userManager.Login(userCredentials.Email, userCredentials.Password);

            if (string.IsNullOrEmpty(token))
            {
                HttpContext.Response.StatusCode = 403;
                return new ObjectResult("The email or password was incorrect!");
            }

            return new JsonResult(token);
        }

        /// <summary>
        /// Update the information of a user in the system
        /// </summary>

        /// <param name="userId">The id of the user to update</param>
        /// <param name="updateUserDTO">The new user information. All internal null values are ignored by the server</param>
        /// <response code="204">The user was successfully updated</response>
        [HttpPut]
        [Route("/1.0.0/users/{userId}")]
        [ValidateModelState]
        [SwaggerOperation("UsersUpdateUser")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult UsersUpdateUser([FromRoute]int? userId, [FromBody]UpdateUserDTO updateUserDTO)
        {
            int callerId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            try
            {
                userManager.UpdateUser(callerId, new UserModel()
                {
                    Id = userId,
                    FirstName = updateUserDTO.FirstName,
                    MiddleInitial = updateUserDTO.MiddleInitial?[0],
                    LastName = updateUserDTO.LastName,
                    Email = updateUserDTO.Email,
                    IsDeleted = updateUserDTO.IsDeleted
                });
            }
            catch (InvalidArgumentException e) 
            {
                switch (e.ParamName)
                {
                    case ParamNameType.CallerId:
                        HttpContext.Response.StatusCode = 403;
                        return new ObjectResult("User not permitted to make changes on users");
                    default:
                        return StatusCode(500);
                }
                throw;
            }

            return StatusCode(204);
        }

        /// <summary>
        /// Get all roles for a user
        /// </summary>
        /// <remarks>Get the list of roles for a specific user</remarks>
        /// <param name="userId"></param>
        /// <response code="200">Successfully retrieved all roles for this user</response>
        [HttpGet]
        [Route("/1.0.0/users/{userId}")]
        [ValidateModelState]
        [SwaggerOperation("UsersGetAllUserRoles")]
        [SwaggerResponse(200, typeof(List<RoleDTO>), "Successfully retrieved all roles for this user")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult GetAllUserRoles([FromRoute]int? userId)
        {
            int callerId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            IEnumerable<RoleModel> roles = userManager.GetAllUserRoles((int)userId);
            IEnumerable<RoleDTO> roleDTOs = roles.Select(x => RoleMapper.MapRoleToRoleDTO(x));
            return new ObjectResult(roleDTOs);
        }
    }
}
