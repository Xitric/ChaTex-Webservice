using Business;
using Business.Models;
using System;

namespace Business.Messages
{
    class MessageManager : IMessageManager
    {
        private readonly IMessageRepository messageRepository;
        private readonly IGroupRepository groupRepository;

        public MessageManager(IMessageRepository messageRepositry, IGroupRepository groupRepository)
        {
            this.messageRepository = messageRepositry;
            this.groupRepository = groupRepository;
        }

        public void CreateMessage(int groupId, int callerId, int channelId, string messageContent)
        {
            GroupUserModel user = groupRepository.GetGroupUser(groupId, callerId);

            if (user != null)
            {
                messageRepository.CreateMessage(new MessageModel()
                {
                    Author = new UserModel { Id = callerId },
                    Content = messageContent,
                }, channelId);
            } else
            {
                throw new Exception("Message couldnt be created, user isnt in the channel group");
            }

        }
    }
}
