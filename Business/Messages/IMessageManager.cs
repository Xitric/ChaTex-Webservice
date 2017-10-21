using Business.Models;
using System.Collections.Generic;

namespace Business.Messages
{
    public interface IMessageManager
    {
        IMessage PostMessage(string content, long authorId);

        IMessage GetMessage(long id);

        List<IGroup> GetGroupsForUser(long userId);
    }
}
