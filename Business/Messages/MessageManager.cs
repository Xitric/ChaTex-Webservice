using Business.Models;
using System;
using System.Collections.Generic;

namespace Business.Messages
{
    class MessageManager : IMessageManager
    {
        private readonly IDataAccess dal;

        public MessageManager(IDataAccess dal)
        {
            this.dal = dal;
        }

        public IMessage GetMessage(long id)
        {
            return dal.GetMessage(id);
        }

        public IMessage PostMessage(string content, long authorId)
        {
            User author = new User(authorId, null, null, null, null);
            Message message = new Message(null, content, author, null);

            return dal.AddMessage(message);
        }

        public List<IGroup> GetGroupsForUser(long userId)
        {
            return dal.GetGroupsForUser(userId);
        }
    }
}
