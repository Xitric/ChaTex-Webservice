﻿/*
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
using WebAPI.Authentication;
using Business.Channels;
using WebAPI.Models;
using System;

namespace IO.Swagger.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ChannelsController : Controller
    {
        private readonly IChannelManager channelManager;

        public ChannelsController(IChannelManager channelManager)
        {
            this.channelManager = channelManager;
        }

        /// <summary>
        /// Create a channel in a group
        /// </summary>
        /// <remarks>Creates a new channel in the specified group</remarks>
        /// <param name="groupId">The id of the group to make the channel in</param>
        /// <param name="name">The name of the new channel</param>
        /// <response code="204">Channel created successfully</response>
        /// <response code="400">Name must be specified</response>
        /// <response code="401">The user was not authorized to access this resource</response>
        /// <response code="404">No group with the specified id was found</response>
        [HttpPost]
        [Route("/1.0.0/groups/{groupId}/channels")]
        [SwaggerOperation("CreateChannel")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult CreateChannel([FromRoute]int? groupId, [FromBody]CreateChannelDTO createChannelDTO)
        {
            int? userId = (int?)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            if (groupId == null)
            {
                return StatusCode(404);
            }

            if (createChannelDTO.ChannelName == null)
            {
                return StatusCode(400);
            }

            bool success = channelManager.CreateChannel((int)groupId, (int)userId, createChannelDTO.ChannelName);

            if (!success)
            {
                return StatusCode(401);
            }

            return StatusCode(204);
        }

        /// <summary>
        /// Delete a channel from a group
        /// </summary>
        /// <remarks>Deletes the channel from the specified group</remarks>
        /// <param name="channelId">The id of the channel to delete</param>
        /// <response code="204">Channel deleted successfully</response>
        /// <response code="401">The user was not authorized to access this resource</response>
        /// <response code="404">No group or channel with the specified ids were found</response>
        [HttpDelete]
        [Route("/1.0.0/channels/{channelId}")]
        [SwaggerOperation("DeleteChannel")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult DeleteChannel([FromRoute]int? channelId) {
            int? userId = (int?)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            if (channelId == null)
            {
                return StatusCode(404);
            }

            channelManager.DeleteChannel((int)userId, (int)channelId);

            return StatusCode(204);
        }

        /// <summary>
        /// Modify a channel in a group
        /// </summary>
        /// <remarks>Modify a channel in a group</remarks>
        /// <param name="channelId">The id of the channel to update</param>
        /// <param name="channelName">The new name of the channel</param>
        /// <response code="204">Channel successfully updated</response>
        /// <response code="401">The user was not authorized to access this resource</response>
        /// <response code="404">No group or channel with the specified ids were found</response>
        [HttpPut]
        [Route("/1.0.0/channels/{channelId}")]
        [SwaggerOperation("UpdateChannel")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult IActionResult([FromRoute]int? channelId, [FromBody]string channelName)
        {
            if (channelId == null || String.IsNullOrEmpty(channelName))
            {
                return StatusCode(404);
;            }

            int? userId = (int?)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            channelManager.UpdateChannel((int)userId, (int)channelId, channelName);

            return StatusCode(204);
        }
    }
}
