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
using Business.Groups;
using Business.Models;
using System.Linq;
using Business.Errors;
using WebAPI.Models.Mappers;
using System;

namespace IO.Swagger.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class GroupsApiController : Controller
    {
        private readonly IGroupManager groupManager;

        public GroupsApiController(IGroupManager groupManager)
        {
            this.groupManager = groupManager;
        }

        /// <summary>
        /// Add access rights for a list of roles to a group
        /// </summary>

        /// <param name="groupId"></param>
        /// <param name="roleIds">The Ids of all the roles</param>
        /// <response code="204">Roles added to group successfully</response>
        [HttpPost]
        [Route("/1.0.0/groups/{groupId}/roles")]
        [ValidateModelState]
        [SwaggerOperation("GroupsAddRolesToGroup")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult GroupsAddRolesToGroup([FromRoute]int? groupId, [FromBody]List<int?> roleIds)
        {
            int callerId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            if (roleIds.Exists(r => r == null))
            {
                return BadRequest("The list of role ids contained illegal data");
            }
            try
            {
                groupManager.AddRolesToGroup((int)groupId, callerId, roleIds.Select(r => r.Value).ToList());
            }
            catch (InvalidArgumentException e)
            {
                switch (e.ParamName)
                {
                    case ParamNameType.CallerId:
                        HttpContext.Response.StatusCode = 403;
                        return new ObjectResult(e.Message);
                    case ParamNameType.GroupId:
                        HttpContext.Response.StatusCode = 403;
                        return new ObjectResult(e.Message);
                    default:
                        return StatusCode(500);
                }
            }

            return StatusCode(204);
        }

        /// <summary>
        /// Add a list of users to a group
        /// </summary>

        /// <param name="groupId"></param>
        /// <param name="users">The list of users to add to the group</param>
        /// <response code="204">Users successfully added to the group</response>
        [HttpPost]
        [Route("/1.0.0/groups/{groupId}/users")]
        [ValidateModelState]
        [SwaggerOperation("GroupsAddUsersToGroup")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult GroupsAddUsersToGroup([FromRoute]int? groupId, [FromBody]List<int?> users)
        {
            int callerId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            if (users.Exists(u => u == null))
            {
                return BadRequest("The list of user ids contained illegal data");
            }

            try
            {
                groupManager.AddUsersToGroup(groupId: (int)groupId,
                                             userIds: users.Select(u => u.Value).ToList(),
                                             callerId: callerId);
            }
            catch (InvalidArgumentException e)
            {
                switch (e.ParamName)
                {
                    case ParamNameType.GroupId:
                        HttpContext.Response.StatusCode = 403;
                        return new ObjectResult(e.Message);
                    case ParamNameType.CallerId:
                        HttpContext.Response.StatusCode = 403;
                        return new ObjectResult(e.Message);
                    default:
                        return StatusCode(500);
                }
            }

            return StatusCode(204);
        }

        /// <summary>
        /// Create a new group
        /// </summary>
        /// <remarks>Creates a new group with the caller as the group administrator. The caller is also initially a member of the group.</remarks>
        /// <param name="createGroupDTO">The initial settings of the group regarding group name and member rights</param>
        /// <response code="200">The group was created, and the group administrator and initial member were successfully added</response>
        [HttpPost]
        [Route("/1.0.0/groups")]
        [ValidateModelState]
        [SwaggerOperation("GroupsCreateGroup")]
        [SwaggerResponse(200, typeof(GroupDTO), "The group was created, and the group administrator and initial member were successfully added")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult GroupsCreateGroup([FromBody]CreateGroupDTO createGroupDTO)
        {
            int callerId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            if (createGroupDTO.GroupName.Length == 0)
            {
                return BadRequest("A group name must be specified");
            }

            int? groupId = groupManager.CreateGroup(callerID: callerId, groupName: createGroupDTO.GroupName,
                                     allowEmployeeSticky: (bool)createGroupDTO.AllowEmployeeSticky,
                                     allowEmployeeAcknowledgeable: (bool)createGroupDTO.AllowEmployeeAcknowledgeable,
                                     allowEmployeeBookmark: (bool)createGroupDTO.AllowEmployeeBookmark);
            return new ObjectResult(new GroupDTO()
            {
                Id = groupId,
                Name = createGroupDTO.GroupName,
                Channels = new List<ChannelDTO>()
            });
        }

        /// <summary>
        /// Delete a group
        /// </summary>

        /// <param name="groupId"></param>
        /// <response code="204">Group deleted successfully</response>
        [HttpDelete]
        [Route("/1.0.0/groups/{groupId}")]
        [ValidateModelState]
        [SwaggerOperation("GroupsDeleteGroup")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult GroupsDeleteGroup([FromRoute]int? groupId)
        {
            int callerId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            try
            {
                groupManager.DeleteGroup((int)groupId, callerId);
            }
            catch (InvalidArgumentException e)
            {
                switch (e.ParamName)
                {
                    case ParamNameType.GroupId:
                        HttpContext.Response.StatusCode = 403;
                        return new ObjectResult(e.Message);
                    case ParamNameType.CallerId:
                        HttpContext.Response.StatusCode = 403;
                        return new ObjectResult(e.Message);
                    default:
                        return StatusCode(500);
                }
            }

            return StatusCode(204);
        }

        /// <summary>
        /// Remove access rights for a list of roles from a group
        /// </summary>

        /// <param name="groupId"></param>
        /// <param name="roleIds">The Ids of all the roles</param>
        /// <response code="204">Roles removed from the group successfully</response>
        [HttpDelete]
        [Route("/1.0.0/groups/{groupId}/roles")]
        [ValidateModelState]
        [SwaggerOperation("GroupsDeleteRolesFromGroup")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult GroupsDeleteRolesFromGroup([FromRoute]int? groupId, [FromBody]List<int?> roleIds)
        {
            int callerId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            if (roleIds.Exists(x => x == null))
            {
                return BadRequest("The list of role ids contained illegal data");
            }

            try
            {
                groupManager.RemoveRolesFromGroup((int)groupId, callerId, roleIds.Select(r => r.Value).ToList());
            }
            catch (InvalidArgumentException e)
            {
                switch (e.ParamName)
                {
                    case ParamNameType.CallerId:
                        HttpContext.Response.StatusCode = 403;
                        return new ObjectResult(e.Message);
                    case ParamNameType.GroupId:
                        HttpContext.Response.StatusCode = 403;
                        return new ObjectResult(e.Message);
                    default:
                        return StatusCode(500);
                }
            }

            return StatusCode(204);
        }

        /// <summary>
        /// Remove a list of users from a group
        /// </summary>

        /// <param name="groupId"></param>
        /// <param name="userIds">The Ids of all the users</param>
        /// <response code="204">Users deleted from the group successfully</response>
        [HttpDelete]
        [Route("/1.0.0/groups/{groupId}/users")]
        [ValidateModelState]
        [SwaggerOperation("GroupsDeleteUsersFromGroup")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult GroupsDeleteUsersFromGroup([FromRoute]int? groupId, [FromBody]List<int?> userIds)
        {
            int callerId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            if (userIds.Exists(u => u == null))
            {
                return BadRequest("The list of user ids contained illegal data");
            }

            try
            {
                groupManager.RemoveUsersFromGroup(groupId: (int)groupId,
                                              userIds: userIds.Select(u => u.Value).ToList(),
                                              callerId: callerId);
            }
            catch (InvalidArgumentException e)
            {
                switch (e.ParamName)
                {
                    case ParamNameType.CallerId:
                        HttpContext.Response.StatusCode = 403;
                        return new ObjectResult(e.Message);
                    case ParamNameType.GroupId:
                        HttpContext.Response.StatusCode = 403;
                        return new ObjectResult(e.Message);
                    default:
                        return StatusCode(500);
                }
            }

            return StatusCode(204);
        }

        /// <summary>
        /// Get a list of admins for a group
        /// </summary>

        /// <param name="groupId"></param>
        /// <response code="200">Successfully retrieved all the admins for the group</response>
        [HttpGet]
        [Route("/1.0.0/groups/{groupId}/admins")]
        [ValidateModelState]
        [SwaggerOperation("GroupsGetAllGroupAdmins")]
        [SwaggerResponse(200, typeof(List<UserDTO>), "Successfully retrieved all the admins for the group")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult GroupsGetAllGroupAdmins([FromRoute]int? groupId)
        {
            int callerId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            try
            {
                IEnumerable<UserModel> admins = groupManager.GetAllGroupAdmins((int)groupId, callerId);
                IEnumerable<UserDTO> dtoResponse = admins.Select(x => UserMapper.MapUserToUserDTO(x, callerId));
                return new ObjectResult(dtoResponse);
            }
            catch (InvalidArgumentException e)
            {
                switch (e.ParamName)
                {
                    case ParamNameType.GroupId:
                        HttpContext.Response.StatusCode = 403;
                        return new ObjectResult(e.Message);
                }
            }

            return StatusCode(500);
        }


        /// <summary>
        /// Get the list of users who have access to a specific group
        /// </summary>
        /// <remarks>This takes into account both what users have direct access to the group and what users have access because of their roles.</remarks>
        /// <param name="groupId"></param>
        /// <response code="200">Successfully retrieved all users in the group</response>
        [HttpGet]
        [Route("/1.0.0/groups/{groupId}/users")]
        [ValidateModelState]
        [SwaggerOperation("GroupsGetAllGroupUsers")]
        [SwaggerResponse(200, typeof(List<UserDTO>), "Successfully retrieved all users in the group")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult GroupsGetAllGroupUsers([FromRoute]int? groupId)
        {
            int callerId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            try
            {
                IEnumerable<UserModel> users = groupManager.GetAllGroupUsers((int)groupId, callerId);
                IEnumerable<UserDTO> dtoResponse = users.Select(x => UserMapper.MapUserToUserDTO(x, callerId));
                return new ObjectResult(dtoResponse);
            }
            catch (InvalidArgumentException e)
            {
                switch (e.ParamName)
                {
                    case ParamNameType.CallerId:
                        HttpContext.Response.StatusCode = 403;
                        return new ObjectResult(e.Message);
                }
            }

            return StatusCode(500);
        }

        /// <summary>
        /// Mark or unmark a user as administrator of the group
        /// </summary>
        /// <remarks>Give a group member administrator rights or remove administrator rights from a group administrator</remarks>
        /// <param name="groupId">The id of the group to affect</param>
        /// <param name="userId">The id of the user to mark or unmark</param>
        /// <param name="isAdministrator">true to mark the user as group administrator, false to unmark</param>
        /// <response code="204">User marked or unmarked successfully</response>
        [HttpPut]
        [Route("/1.0.0/groups/{groupId}/{userId}")]
        [ValidateModelState]
        [SwaggerOperation("GroupsMarkUserAsAdministrator")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult GroupsMarkUserAsAdministrator([FromRoute]int? groupId, [FromRoute]int? userId, [FromBody]bool? isAdministrator)
        {
            int callerId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            try
            {
                groupManager.SetUserAdministratorOnGroup((int)groupId, (int)userId, callerId, (bool)isAdministrator);
            }
            catch (InvalidArgumentException e)
            {
                switch (e.ParamName)
                {
                    case ParamNameType.CallerId:
                        HttpContext.Response.StatusCode = 403;
                        return new ObjectResult(e.Message);
                    case ParamNameType.GroupId:
                        HttpContext.Response.StatusCode = 403;
                        return new ObjectResult(e.Message);
                    default:
                        return StatusCode(500);
                }
            }

            return StatusCode(204);
        }

        /// <summary>
        /// Change group name
        /// </summary>

        /// <param name="groupId"></param>
        /// <param name="groupName"></param>
        /// <response code="204">Group name successfully updated</response>
        [HttpPut]
        [Route("/1.0.0/groups/{groupId}")]
        [ValidateModelState]
        [SwaggerOperation("GroupsUpdateGroup")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult GroupsUpdateGroup([FromRoute]int? groupId, [FromBody]string groupName)
        {
            int callerId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            try
            {
                //TODO: WTF? Don't we need to check if the user is a group admin???
                groupManager.UpdateGroup((int)groupId, groupName);
            }
            catch (InvalidArgumentException e)
            {
                switch (e.ParamName)
                {
                    case ParamNameType.CallerId:
                        HttpContext.Response.StatusCode = 403;
                        return new ObjectResult(e.Message);
                    case ParamNameType.GroupId:
                        HttpContext.Response.StatusCode = 403;
                        return new ObjectResult(e.Message);
                    default:
                        return StatusCode(500);
                }
            }

            return StatusCode(204);
        }

        /// <summary>
        /// Get all the the users with direct access to a specific group.
        /// </summary>
        /// <remarks>Gets all users with direct access to the group, this will not get the users added by roles.</remarks>
        /// <param name="groupId"></param>
        /// <response code="200">Successfully retrieved all direct users in the group</response>
        [HttpGet]
        [Route("/1.0.0/groups/{groupId}/groupusers")]
        [ValidateModelState]
        [SwaggerOperation("GetAllDirectGroupUsers")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        [SwaggerResponse(200, typeof(List<UserDTO>), "Successfully retrieved all direct users in the group")]
        public virtual IActionResult GetAllDirectGroupUsers([FromRoute]int? groupId)
        {
            int callerId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];
            if (groupId == null)
            {
                return BadRequest("Bad input");
            }

            try
            {
                IEnumerable<UserModel> users = groupManager.GetAllDirectGroupUsers((int)groupId, callerId);
                IEnumerable<UserDTO> userDTOs = users.Select(x => UserMapper.MapUserToUserDTO(x, callerId));
                return new ObjectResult(userDTOs);
            }
            catch (InvalidArgumentException e)
            {
                switch (e.ParamName)
                {
                    case ParamNameType.GroupId:
                        HttpContext.Response.StatusCode = 403;
                        return new ObjectResult(e.Message);
                }
            }

            return StatusCode(500);
        }

        /// <summary>
        /// Get the list of roles that have access to a specific group
        /// </summary>

        /// <param name="groupId"></param>
        /// <response code="200">Successfully retrieved all roles in the group</response>
        [HttpGet]
        [Route("/1.0.0/groups/{groupId}/roles")]
        [ValidateModelState]
        [SwaggerOperation("GroupsGetAllGroupRoles")]
        [SwaggerResponse(200, typeof(List<RoleDTO>), "Successfully retrieved all roles in the group")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult GetAllGroupRoles([FromRoute]int? groupId)
        {
            int callerId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];
            if (groupId == null)
            {
                return BadRequest("Bad input");
            }

            try
            {
                IEnumerable<RoleModel> roles = groupManager.GetAllGroupRoles((int)groupId);
                IEnumerable<RoleDTO> roleDTOs = roles.Select(x => RoleMapper.MapRoleToRoleDTO(x));
                return new ObjectResult(roleDTOs);
            }
            catch (InvalidArgumentException e)
            {
                switch (e.ParamName)
                {
                    case ParamNameType.GroupId:
                        HttpContext.Response.StatusCode = 403;
                        return new ObjectResult(e.Message);
                }
            }
            return StatusCode(500);
        }

    }

}
