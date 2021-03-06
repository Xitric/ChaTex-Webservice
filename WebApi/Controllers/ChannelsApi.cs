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

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using IO.Swagger.Attributes;
using IO.Swagger.Models;
using Business.Channels;
using WebAPI.Authentication;
using Business.Errors;
using Business.Models;
using System.Threading;
using System.Linq;
using WebAPI.Models.Mappers;

namespace IO.Swagger.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ChannelsApiController : Controller
    {
        private readonly IChannelManager channelManager;

        public ChannelsApiController(IChannelManager channelManager)
        {
            this.channelManager = channelManager;
        }

        /// <summary>
        /// Create a channel in a group
        /// </summary>

        /// <param name="groupId"></param>
        /// <param name="channelName"></param>
        /// <response code="204">Channel created successfully</response>
        [HttpPost]
        [Route("/1.0.0/groups/{groupId}/channels")]
        [ValidateModelState]
        [SwaggerOperation("ChannelsCreateChannel")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult ChannelsCreateChannel([FromRoute]int? groupId, [FromBody]string channelName)
        {
            int callerId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            if (channelName.Length == 0)
            {
                return BadRequest("A channel name must be specified");
            }

            try
            {
                channelManager.CreateChannel((int)groupId, callerId, channelName);
            }
            catch (InvalidArgumentException e)
            {
                switch (e.ParamName)
                {
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
        /// Delete a channel from a group
        /// </summary>

        /// <param name="channelId">The id of the channel to delete</param>
        /// <response code="204">Channel deleted successfully</response>
        [HttpDelete]
        [Route("/1.0.0/channels/{channelId}")]
        [ValidateModelState]
        [SwaggerOperation("ChannelsDeleteChannel")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult ChannelsDeleteChannel([FromRoute]int? channelId)
        {
            int callerId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            try
            {
                channelManager.DeleteChannel(callerId, (int)channelId);
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
                    case ParamNameType.ChannelId:
                        return NotFound(e.Message);
                    default:
                        return StatusCode(500);
                }
            }

            return StatusCode(204);
        }

        /// <summary>
        /// Wait for and get new messages, message deletions, message edits, channel information updates, and channel deletions in a channel
        /// </summary>
        /// <remarks>This request will not return from the service until at least one new channel event has occurred</remarks>
        /// <param name="channelId">The id of the channel to listen to</param>
        /// <param name="since">The time to get channel events since</param>
        /// <response code="200">Channel events fetched successfully</response>
        [HttpGet]
        [Route("/1.0.0/channels/{channelId}/events")]
        [ValidateModelState]
        [SwaggerOperation("ChannelsGetChannelEvents")]
        [SwaggerResponse(200, typeof(List<ChannelEventDTO>), "Channel events fetched successfully")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult ChannelsGetChannelEvents([FromRoute]int? channelId, [FromQuery]DateTime? since, CancellationToken cancellation)
        {
            int callerId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            try
            {
                IEnumerable<ChannelEventModel> channelEvents = channelManager.GetChannelEvents((int)channelId, callerId, (DateTime)since, cancellation);

                if (channelEvents == null)
                {
                    //The client canceled the request
                    return NoContent();
                }

                IEnumerable<ChannelEventDTO> channelEventDTOs = channelEvents.Select(ce => ChannelMapper.MapChannelEventToChannelEventDTO(ce, callerId));

                return new ObjectResult(channelEventDTOs);
            }
            catch (InvalidArgumentException e)
            {
                switch (e.ParamName)
                {
                    case ParamNameType.CallerId:
                        HttpContext.Response.StatusCode = 403;
                        return new ObjectResult(e.Message);
                    default:
                        //Some unexpected exception
                        return StatusCode(500);
                }
            }
        }

        /// <summary>
        /// Change the information about a channel in a group
        /// </summary>

        /// <param name="channelId">The id of the channel to update</param>
        /// <param name="channelName">The new name of the channel</param>
        /// <response code="204">Channel information updated successfully</response>
        [HttpPut]
        [Route("/1.0.0/channels/{channelId}")]
        [ValidateModelState]
        [SwaggerOperation("ChannelsUpdateChannel")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult ChannelsUpdateChannel([FromRoute]int? channelId, [FromBody]string channelName)
        {
            int callerId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            if (channelName.Length == 0)
            {
                return BadRequest("A new channel name must be specified");
            }

            try
            {
                channelManager.UpdateChannel(callerId, (int)channelId, channelName);
            }
            catch (InvalidArgumentException e)
            {
                switch (e.ParamName)
                {
                    case ParamNameType.ChannelId:
                        return NotFound("Channel does not exist");
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
    }
}
