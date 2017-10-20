using Business.Models;
using System;

namespace Business.Messages
{
    public interface IModelFactory
    {
        IMessage CreateMessage(long? id, string content, IUser author, DateTime? creationTime);

        IUser CreateUser(long? id, string firstName, char? middleInitial, string lastName, string email);
    }
}
