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

using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using Business.Groups;
using System;
using WebAPI.Authentication;
using System.Collections.Generic;
using WebAPI.Models;
using WebAPI.Models.Mappers;
using Business.Models;

namespace WebAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class GroupsController : Controller
    {
        private readonly IGroupManager groupManager;

        public GroupsController(IGroupManager groupManager)
        {
            this.groupManager = groupManager;
        }

        /// <summary>
        /// Create a new group
        /// </summary>
        /// <remarks>Creates a new group with the caller as the group administrator</remarks>
        /// <param name="groupName">The name of the group</param>
        /// <param name="allowEmployeeSticky">Whether employees are allowed to make sticky messages</param>
        /// <param name="allowEmployeeAcknowledgeable">Whether employees are allowed to make acknowledgeable messages</param>
        /// <param name="allowEmployeeBookmark">Whether employees are allowed to make bookmarks</param>
        /// <response code="204">The group was created, and the group administrator was successfully added</response>
        /// <response code="400">Bad input</response>
        /// <response code="401">The user was not authorized to access this resource</response>
        [HttpPost]
        [Route("/1.0.0/groups")]
        [SwaggerOperation("CreateGroup")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        [SwaggerResponse(200, type: typeof(GroupDTO))]
        public virtual IActionResult CreateGroup([FromBody]CreateGroupDTO createGroupDTO)
        {
            int? userId = (int?)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            if (string.IsNullOrEmpty(createGroupDTO.GroupName))
            {
                return StatusCode(400);
            }

            int? groupId = groupManager.CreateGroup(userId: (int)userId, groupName: createGroupDTO.GroupName,
                                     allowEmployeeSticky: (bool)createGroupDTO.AllowEmployeeSticky,
                                     allowEmployeeAcknowledgeable: (bool)createGroupDTO.AllowEmployeeAcknowledgeable,
                                     allowEmployeeBookmark: (bool)createGroupDTO.AllowEmployeeBookmark);
            return new ObjectResult(new GroupDTO(groupId, createGroupDTO.GroupName, new List<ChannelDTO>()));

        }

        /// <summary>
        /// Get users for a group
        /// </summary>
        /// <remarks>Get users, taking into account members and roles on the group</remarks>
        /// <param name="groupId"></param>
        /// <response code="200">Successfully retrieved all the groupuser&#39;s</response>
        /// <response code="401">The user was not authorized to access this resource</response>
        /// <response code="404">No group with the specified id was found</response>
        [HttpGet]
        [Route("/1.0.0/groups/{groupId}/users")]
        [SwaggerOperation("GetAllGroupUsers")]
        [SwaggerResponse(200, type: typeof(List<UserDTO>))]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult GetAllGroupUsers([FromRoute]int? groupId)
        {
            int? userId = (int?)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            if (groupId == null)
            {
                return StatusCode(404);
            }
            try
            {
                IEnumerable<UserModel> users = groupManager.GetAllGroupUsers((int)groupId,(int)userId);
                IEnumerable<UserDTO> dtoResponse = users.Select(x => UserMapper.MapUserToUserDTO(x, (int)userId));
                return new ObjectResult(dtoResponse);
            } catch(Exception)
            {
                return StatusCode(401);
            }
        }

        /// <summary>
        /// Delete a group
        /// </summary>
        /// <remarks>Deletes the group with the specified id</remarks>
        /// <param name="groupId"></param>
        /// <response code="204">Group deleted successfully</response>
        /// <response code="404">No group with the specified id exists</response>
        /// <response code="401">The user was not authorized to access this resource</response>
        [HttpDelete]
        [Route("/1.0.0/groups/{groupId}")]
        [SwaggerOperation("DeleteGroup")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult DeleteGroup([FromRoute]int? groupId)
        {
            int? userId = (int?)HttpContext.Items[ChaTexAuthorization.UserIdKey];
            
            if (groupId == null)
            {
                return StatusCode(404);
            }
            try
            {
                groupManager.DeleteGroup((int)groupId, (int)userId);
                return StatusCode(204);
            } catch(Exception)
            {
                return StatusCode(401);
            }
        }

        /// <summary>
        /// Add users to a group
        /// </summary>
        /// <remarks>This will add a list of users to a specific group</remarks>
        /// <param name="groupId">The Id of the group</param>
        /// <param name="userIds">The Ids of all the users</param>
        /// <response code="204">Users added to group successfully</response>
        /// <response code="401">The user was not authorized to access this resource</response>
        /// <response code="404">No group or user with the specified ids exists</response>
        [HttpPost]
        [Route("/1.0.0/groups/users")]
        [SwaggerOperation("AddUsersToGroup")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult AddUsersToGroup([FromBody]AddUsersToGroupDTO addUsersToGroupDTO)
        {
            int? userId = (int?)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            //If our list of users is null, or if it contains any element that is null
            if (addUsersToGroupDTO.GroupId == null || addUsersToGroupDTO.UserIds == null || addUsersToGroupDTO.UserIds.Exists(x => x == null))
            {
                return StatusCode(404);
            }

            //Add user (also convert list of nullable ints, to list of ints)
            try
            {
                groupManager.AddUsersToGroup(groupId: (int)addUsersToGroupDTO.GroupId,
                                             userIds: addUsersToGroupDTO.UserIds.Where(x => x != null).Select(x => x.Value).ToList(),
                                             loggedInUser: (int)userId);
            }
            catch (Exception)
            {
                return StatusCode(403);
            }
            return StatusCode(204);

        }

        /// <summary>
        /// Delete a list of users from a group
        /// </summary>
        /// <remarks>This will delete a list of users from the specific group</remarks>
        /// <param name="groupId">The Id of the group</param>
        /// <param name="userIds">The Ids of all the users</param>
        /// <response code="204">Users deleted from the group successfully</response>
        /// <response code="401">The user was not authorized to access this resource</response>
        /// <response code="404">No group or user with the specified ids exists</response>
        [HttpDelete]
        [Route("/1.0.0/groups/users")]
        [SwaggerOperation("DeleteUsersFromGroup")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult DeleteUsersFromGroup([FromQuery]int? groupId, [FromBody]List<int?> userIds)
        {
            int? userId = (int?)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            //If our list of users is null, or if it contains any element that is null
            if (groupId == null || userIds == null || userIds.Exists(x => x == null))
            {
                return StatusCode(404);
            }

            //Add user (also convert list of nullable ints, to list of ints)
            groupManager.RemoveUsersFromGroup(groupId: (int)groupId, userIds: userIds.Where(x => x != null).Select(x => x.Value).ToList(), loggedInUserId: (int)userId);
            return StatusCode(204);
        }

        /// <summary>
        /// Add access rights for roles to a group
        /// </summary>
        /// <remarks>This will add access rights for a list of roles to a specific group</remarks>
        /// <param name="groupId">The Id of the group</param>
        /// <param name="roleIds">The Ids of all the roles</param>
        /// <response code="204">Roles added to group successfully</response>
        /// <response code="401">The user was not authorized to access this resource</response>
        /// <response code="404">No group or role with the specified ids exists</response>
        [HttpPost]
        [Route("/1.0.0/groups/roles")]
        [SwaggerOperation("AddRolesToGroup")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult AddRolesToGroup([FromBody]AddRolesToGroupDTO addRolesToGroupDTO)
        {
            int? userId = (int?)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            if (addRolesToGroupDTO.GroupId == null || userId == null || addRolesToGroupDTO.RoleIds == null || addRolesToGroupDTO.RoleIds.Exists(x => x == null))
            {
                return StatusCode(404);
            }
            try
            {
                groupManager.AddRolesToGroup((int)addRolesToGroupDTO.GroupId, (int)userId, addRolesToGroupDTO.RoleIds.Where(x => x != null).Select(x => x.Value).ToList());
            }
            catch (Exception)
            {
                return StatusCode(401);
            }
            return StatusCode(200);
        }

        /// <summary>
        /// Remove access rights for roles from a group
        /// </summary>
        /// <remarks>This will remove access for a list of roles from a specific group</remarks>
        /// <param name="groupId">The Id of the group</param>
        /// <param name="roleIds">The Ids of all the roles</param>
        /// <response code="204">Roles deleted from the group successfully</response>
        /// <response code="401">The user was not authorized to access this resource</response>
        /// <response code="404">No group or role with the specified ids exists</response>
        [HttpDelete]
        [Route("/1.0.0/groups/roles")]
        [SwaggerOperation("DeleteRolesFromGroup")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult DeleteRolesFromGroup([FromQuery]int? groupId, [FromBody]List<int?> roleIds)
        {
            int? userId = (int?)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            if (groupId == null || userId == null || roleIds == null || roleIds.Exists(x => x == null))
            {
                return StatusCode(404);
            }
            try
            {
                groupManager.RemoveRolesFromGroup((int)groupId, (int)userId, roleIds.Where(x => x != null).Select(x => x.Value).ToList());
            }
            catch (Exception)
            {
                return StatusCode(401);
            }
            return StatusCode(200);
        }

        /// <summary>
        /// Mark or unmark a user as administrator
        /// </summary>
        /// <remarks>Give a group member administrator rights or remove administrator rights from a group administrator</remarks>
        /// <param name="groupId">The id of the group to affect</param>
        /// <param name="userId">The id of the user to mark or unmark</param>
        /// <param name="isAdministrator">true to mark the user as group administrator, false to unmark</param>
        /// <response code="200">User marked or unmarked successfully</response>
        /// <response code="401">The user was not authorized to access this resource</response>
        /// <response code="404">No group or group user with the specified ids were found</response>
        [HttpPut]
        [Route("/1.0.0/groups/{groupId}/{userId}")]
        [SwaggerOperation("MarkUserAsAdministrator")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult MarkUserAsAdministrator([FromRoute]int? groupId, [FromRoute]int? userId, [FromQuery]bool? isAdministrator)
        {
            int? loggedInUserId = (int?)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            if (groupId == null || loggedInUserId == null || userId == null || isAdministrator == null)
            {
                return StatusCode(404);
            }
            try
            {
                groupManager.SetUserAdministratorOnGroup((int)groupId, (int)userId, (int)loggedInUserId, (bool)isAdministrator);
            }
            catch (Exception)
            {
                return StatusCode(401);
            }
            return StatusCode(200);
        }

        /// <summary>
        /// Update group name
        /// </summary>
        /// <remarks>update the group name with the specified id</remarks>
        /// <param name="groupId"></param>
        /// <param name="groupName"></param>
        /// <response code="204">Group name successfully updated</response>
        /// <response code="401">The user was not authorized to access this resource</response>
        /// <response code="404">No group with the specified id exists</response>
        [HttpPut]
        [Route("/1.0.0/groups/{groupId}")]
        [SwaggerOperation("UpdateGroup")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult UpdateGroup([FromRoute]int? groupId, [FromQuery]string groupName)
         {
            int? loggedInUserId = (int?)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            if(groupId == null || loggedInUserId == null || groupName == null )
            {
                return StatusCode(404);
            }
            try
            {
                groupManager.UpdateGroup((int)groupId, (string)groupName);
            }
            catch (Exception)
            {
                return StatusCode(401);
            }
            return StatusCode(200);
        }
    }
}
