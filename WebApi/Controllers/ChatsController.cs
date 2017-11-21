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
using Newtonsoft.Json;
using IO.Swagger.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebAPI.Authentication;
using Business.Chats;
using System.Linq;

namespace IO.Swagger.Controllers
{
    
    public class ChatsController : Controller
    {
        private readonly IChatManager chatManager;

        public ChatsController(IChatManager chatManager)
        {
            this.chatManager = chatManager;
        }
        /// <summary>
        /// Add users to a chat
        /// </summary>
        /// <remarks>This will add a list of users to a specific chat</remarks>
        /// <param name="addUsersToChatDTO">Users to be added to a chat</param>
        /// <response code="204">Users added to chat successfully</response>
        /// <response code="401">The user was not authorized to access this resource</response>
        /// <response code="404">No chat or user with the specified ids exists</response>
        [HttpPost]
        [Route("/1.0.0/chats/users")]
        [SwaggerOperation("AddUsersToChat")]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult AddUsersToChat([FromBody]AddUsersToChatDTO addUsersToChatDTO)
        {
            int callerId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];
            
            try
            {
                chatManager.AddUsersToChat(chatId: (int)addUsersToChatDTO.ChatId,
                userIds: addUsersToChatDTO.UserIds.Where(x => x != null).Select(x => x.Value).ToList());
            }
            catch (Exception)
            {
                //TODO:change
                return StatusCode(403);
            }

            return StatusCode(204);
        }


        /// <summary>
        /// Create a new chat
        /// </summary>
        /// <remarks>Creates a new chat</remarks>
        /// <param name="createChatDTO">The name of the group</param>
        /// <response code="200">The chat was created</response>
        /// <response code="400">Bad input</response>
        /// <response code="401">The user was not authorized to access this resource</response>
        [HttpPost]
        [Route("/1.0.0/chats")]
        [SwaggerOperation("CreateChat")]
        [SwaggerResponse(200, type: typeof(ChatDTO))]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult CreateChat([FromBody]CreateChatDTO createChatDTO)
        {
            int callerId = (int)HttpContext.Items[ChaTexAuthorization.UserIdKey];

            if (string.IsNullOrEmpty(createChatDTO.ChatName))
            {
                return StatusCode(400);
            }

            try
            {
                int? chatId = chatManager.CreateChat(userId: callerId, chatName: createChatDTO.ChatName);

                if (chatId == null)
                {
                    return StatusCode(401);
                }

                return new ObjectResult(new ChatDTO(chatId, createChatDTO.ChatName, new List<UserDTO>()));
            }
            catch (Exception)
            {
                //TODO:change
                return StatusCode(401);
            }
        }


        /// <summary>
        /// Get alle chats for the user
        /// </summary>
        /// <remarks>Gets all chats for the specific user id</remarks>
        /// <param name="userId">The id of the user</param>
        /// <response code="200">Chats was returned successfully</response>
        /// <response code="401">The user was not authorized to access this resource</response>
        /// <response code="404">Could not find the chat with the specified id</response>
        [HttpGet]
        [Route("/1.0.0/chats/users/{userId}")]
        [SwaggerOperation("GetAllChatsForUser")]
        [SwaggerResponse(200, type: typeof(List<ChatDTO>))]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult GetAllChatsForUser([FromRoute]int? userId)
        {
            throw new NotImplementedException("This feature is not yet implemented.");
        }


        /// <summary>
        /// Get all messages in a one-to-one chat
        /// </summary>
        /// <remarks>This will get a list of messages in a chat from a specific user</remarks>
        /// <param name="chatId">The id of the message to get</param>
        /// <response code="200">Messages was returned successfully</response>
        /// <response code="401">The user was not authorized to access this resource</response>
        /// <response code="404">Could not find the chat with the specified id</response>
        [HttpGet]
        [Route("/1.0.0/chats/{chatId}")]
        [SwaggerOperation("GetMessagesInChat")]
        [SwaggerResponse(200, type: typeof(List<GetMessageDTO>))]
        [ServiceFilter(typeof(ChaTexAuthorization))]
        public virtual IActionResult GetMessagesInChat([FromRoute]int? chatId)
        {
            throw new NotImplementedException("This feature is not yet implemented.");
        }
    }
}
