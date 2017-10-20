using System;
using Business.Models;

namespace Business.Messages
{
    class ModelFactory : IModelFactory
    {
        public IMessage CreateMessage(long? id, string content, IUser author, DateTime? creationTime)
        {
            return new Message(id, content, author, creationTime);
        }

        public IUser CreateUser(long? id, string firstName, char? middleInitial, string lastName, string email)
        {
            return new User(id, firstName, middleInitial, lastName, email);
        }
    }
}
