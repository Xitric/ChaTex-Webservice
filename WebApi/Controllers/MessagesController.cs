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

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Business.Messages;
using WebAPI.Models;
using WebAPI.Authentication;
using System.Linq;
using WebAPI.Models.Mappers;
using System;
using Business.Models;
using System.Threading;
using IO.Swagger.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly IMessageManager messageManager;

        public MessagesController(IMessageManager messageManager)
        {
            this.messageManager = messageManager;
        }

        /// <summary>
        /// Get the messages from a specific channel
        /// </summary>
        /// <remarks>Get a number of messages from the specified channel</remarks>
        /// <param name="channelId">The id of the channel to delete</param>
        /// <param name="fromIndex">The index of the first message to get, beginning from the most recently posted message. This defaults to 0, meaning the most recent message</param>
        /// <param name="count">The amount of messages to get. This defaults to 25</param>
        /// <response code="200">Messages fetched successfully</response>
        /// <response code="401">The user was not authorized to access this resource</response>
        /// <response code="404">Could not find the channel with the specified id</response>
        [HttpGet]
        [Route("/1.0.0/channels/{channelId}/messages")]
        [SwaggerOperation("GetMessages")]
        [SwaggerResponse(200, type: typeof(GetMessageDTO))]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult GetMessages([FromRoute]int? channelId, [FromQuery]int? fromIndex, [FromQuery]int? count)
        {
            int userId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            if (fromIndex == null) fromIndex = 0;
            if (count == null) count = 25;

            if (channelId == null)
            {
                return BadRequest("Channel id must be specified");
            }

            try
            {
                IEnumerable<GetMessageDTO> messages = messageManager.GetMessages((int)channelId, userId, (int)fromIndex, (int)count)
                .Select(m => MessageMapper.MapMessageToGetMessageDTO(m, userId));

                return new ObjectResult(messages);
            }
            catch (ArgumentException e)
            {
                switch (e.ParamName)
                {
                    case "callerId":
                        //Caller was not authorized
                        return new StatusCodeResult(401);
                    case "channelId":
                        return NotFound($"The channel with id {channelId} was not found");
                    default:
                        //Some unexpected exception
                        throw;
                }
            }
        }

        /// <summary>
        /// Get a message
        /// </summary>
        /// <remarks>Get a message with the specified id</remarks>
        /// <param name="messageId">The id of the message to get</param>
        /// <response code="200">Message was returned successfully</response>
        /// <response code="401">The user was not authorized to access this resource</response>
        /// <response code="404">Could not find the message with the specified id</response>
        [HttpGet]
        [Route("/1.0.0/messages/{messageId}")]
        [SwaggerOperation("GetMessage")]
        [SwaggerResponse(200, type: typeof(GetMessageDTO))]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult GetMessage([FromRoute]int? messageId)
        {
            int userId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];
            if (messageId == null)
            {
                return BadRequest("Message id must be specified");
            }
            try
            {
                GetMessageDTO message = MessageMapper.MapMessageToGetMessageDTO(messageManager.GetMessage(userId, (int)messageId), userId);
                return new ObjectResult(message);
            }

            catch (ArgumentException e)
            {
                switch (e.ParamName)
                {
                    case "callerId":
                        //Caller was not authorized
                        return new StatusCodeResult(401);
                    case "messageId":
                        return NotFound($"The message with id {messageId} was not found");
                    default:
                        //Some unexpected exception
                        throw;
                }
            }

        }

        /// <summary>
        /// Wait for and get new messages, message deletions, and message edits in a channel
        /// </summary>
        /// <remarks>This request will not return from the service until at least one new message event has occurred</remarks>
        /// <param name="channelId">The id of the channel to listen to</param>
        /// <param name="since">The time to get message events since</param>
        /// <response code="200">Message events fetched successfully</response>
        /// <response code="401">The user was not authorized to access this resource</response>
        /// <response code="404">Could not find the channel with the specified id</response>
        [HttpGet]
        [Route("/1.0.0/channels/{channelId}/messages/live")]
        [SwaggerOperation("GetMessageEvents")]
        [SwaggerResponse(200, type: typeof(List<MessageEventDTO>))]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult GetMessageEvents([FromRoute]int? channelId, [FromQuery]DateTime? since, CancellationToken cancellation)
        {
            int userId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            if (channelId == null || since == null)
            {
                return BadRequest("Channel id and date must be specified");
            }

            try
            {
                IEnumerable<MessageEventModel> messageEvents = messageManager.GetMessageEvents((int)channelId, userId, (DateTime)since, cancellation);
                if (messageEvents == null) return NoContent(); //The client canceled the request

                IEnumerable<MessageEventDTO> messageEventDTO = messageEvents.Select(me => MessageMapper.MapMessageEventToMessageEventDTO(me, userId));
                return new ObjectResult(messageEventDTO);
            }
            catch (ArgumentException e)
            {
                switch (e.ParamName)
                {
                    case "callerId":
                        //Caller was not authorized
                        return new StatusCodeResult(401);
                    default:
                        //Some unexpected exception
                        throw;
                }
            }
        }

        /// <summary>
        /// Create a new message
        /// </summary>
        /// <remarks>Create a new message in a specific channel</remarks>
        /// <param name="channelId">The id of the channel to delete</param>
        /// <param name="messageContent">Content of the message</param>
        /// <response code="204">Messages was successfully posted.</response>
        /// <response code="401">The user was not authorized to access this resource</response>
        /// <response code="404">No group or channel with the specified ids were found</response>
        [HttpPost]
        [Route("/1.0.0/channels/{channelId}/messages")]
        [SwaggerOperation("CreateMessage")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult CreateMessage([FromRoute]int? channelId, [FromBody]MessageContentDTO messageContentDTO)
        {
            int? userId = (int?)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            if (channelId == null || String.IsNullOrEmpty(messageContentDTO.Message))
            {
                return BadRequest("Channel id and message content must be specified");
            }

            try
            {
                messageManager.CreateMessage((int)userId, (int)channelId, messageContentDTO.Message);
                return NoContent();
            }
            catch (ArgumentException e)
            {
                switch (e.ParamName)
                {
                    case "callerId":
                        //Caller was not authorized
                        return new StatusCodeResult(401);
                    default:
                        //Some unexpected exception
                        throw;
                }
            }
        }


        /// <summary>
        /// Delete a message
        /// </summary>
        /// <remarks>Delete the message with the specified id</remarks>
        /// <param name="messageId">The id of the message to delete</param>
        /// <response code="204">Message deleted successfully</response>
        /// <response code="401">The user was not authorized to access this resource</response>
        /// <response code="404">Could not find the message with the specified id</response>
        [HttpDelete]
        [Route("/1.0.0/messages/{messageId}")]
        [SwaggerOperation("DeleteMessage")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult DeleteMessage([FromRoute]int? messageId)
        {
            int userId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];
            if (messageId == null)
            {
                return BadRequest("Message id must be specified");
            }

            try
            {
                messageManager.DeleteMessage(userId, (int)messageId);
                return NoContent();
            }
            catch (ArgumentException e)
            {
                switch (e.ParamName)
                {
                    case "callerId":
                        //Caller was not authorized
                        return new StatusCodeResult(401);
                    case "messageId":
                        //Message was unknown
                        return NotFound($"The message with id {messageId} was not found");
                    default:
                        //Some unexpected exception
                        throw;
                }
            }
        }
        /// <summary>
        /// Edit a message
        /// </summary>
        /// <remarks>Edit the message with the specified id</remarks>
        /// <param name="messageId">The id of the message to delete</param>
        /// <param name="newContent">The new content of the message</param>
        /// <response code="204">Message edited successfully</response>
        /// <response code="401">The user was not authorized to access this resource</response>
        /// <response code="404">Could not find the message with the specified id</response>
        [HttpPut]
        [Route("/1.0.0/messages/{messageId}")]
        [SwaggerOperation("EditMessage")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult EditMessage([FromRoute]int? messageId, [FromBody]string newContent)
        {
            int userId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];
            if (messageId == null || String.IsNullOrEmpty(newContent))
            {
                return BadRequest($"Message id and message content must be specified!");
            }

            try
            {
                messageManager.EditMessage(userId, (int)messageId, newContent);
                return NoContent();
            }
            catch (ArgumentException e)
            {
                switch (e.ParamName)
                {
                    case "callerId":
                        //Caller was not authorized
                        return new StatusCodeResult(401);
                    case "messageId":
                        //Message was unknown
                        return NotFound($"The message with id {messageId} was not found");
                    default:
                        //Some unexpected exception
                        throw;
                }
            }
        }
    }
}
