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

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Business.Messages;
using WebAPI.Models;
using WebAPI.Mappers;
using Business.Models;
using WebAPI.Authentication;
using System.Linq;
using WebAPI.Models.Mappers;

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
                return StatusCode(404);
            }

            IEnumerable<GetMessageDTO> messages = messageManager.GetMessages((int)channelId, userId, (int)fromIndex, (int)count)
                .Select(m => MessageMapper.MapMessageToGetMessageDTO(m, userId));

            return new ObjectResult(messages);
        }
    }
}
