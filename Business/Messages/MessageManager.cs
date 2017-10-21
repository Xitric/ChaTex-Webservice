using Business.Models;

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
            return dal.AddMessage(message);
        }
    }
}
