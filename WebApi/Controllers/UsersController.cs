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
using WebAPI.Models;
using Business.Messages;
using System.Linq;
using Business.Authentication;
using Microsoft.AspNetCore.Http;
using Business.Models;
using WebAPI.Authentication;
using Business.Users;
using System.Net;
using Newtonsoft.Json;
using WebAPI.Models.Mappers;

namespace WebAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class UsersController : Controller
    {
        private readonly IMessageManager messageManager;
        private readonly IUserManager userManager;

        public UsersController(IMessageManager messageManager, IUserManager userManager)
        {
            this.messageManager = messageManager;
            this.userManager = userManager;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <remarks>Get the available users in the system</remarks>
        /// <response code="200">Successfully retrieved all the user's</response>
        /// <response code="401">The user was not authorized to access this resource</response>
        [HttpGet]
        [Route("/1.0.0/users")]
        [SwaggerOperation("GetAllUsers")]
        [SwaggerResponse(200, type: typeof(List<UserDTO>))]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult GetAllUsers()
        {
            int? userId = (int?)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            IEnumerable<UserModel> users = userManager.GetAllUsers();
            IEnumerable<UserDTO> dtoResponse = users.Select(u => UserMapper.MapUserToUserDTO(u, (int)userId));
           
            return new ObjectResult(dtoResponse);
        }

        /// <summary>
        /// Get the available groups to a user.
        /// </summary>
        /// <remarks>Get the available groups to the user with the specified ID.</remarks>
        /// <response code="200">Successfully retrieved the user&#39;s groups</response>
        /// <response code="401">The user was not authorized to access this resource</response>
        [HttpGet]
        [Route("/1.0.0/users/me/groups")]
        [SwaggerOperation("GetGroupsForUser")]
        [SwaggerResponse(200, type: typeof(List<GroupDTO>))]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult GetGroupsForUser()
        {
            int? userId = (int?)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            IEnumerable<GroupModel> groups = userManager.GetGroupsForUser((int)userId);
            List<GroupDTO> dtoResponse = groups.Select(g => GroupMapper.MapGroupToGroupDTO(g)).ToList();

            return new ObjectResult(dtoResponse);
        }

        /// <summary>
        /// Login a user
        /// </summary>
        /// <remarks>Login the user with the specified e-mail</remarks>
        /// <param name="userEmail">The user&#39;s email</param>
        /// <response code="200">The user was successfully logged in</response>
        /// <response code="401">No user with the specified e-mail was found</response>
        [HttpGet]
        [Route("/1.0.0/users/login")]
        [SwaggerOperation("Login")]
        [SwaggerResponse(200, type: typeof(string))]
        public virtual IActionResult Login([FromQuery]string userEmail)
        {
            if (userEmail == null)
            {
                return BadRequest("An email must be specified!");
            }

            string token = userManager.Login(userEmail);

            if (token == null)
            {
                return NotFound("No user with the specified email was found!");
            }
            
            return Content(token);
        }
    }
}
