﻿namespace Business.Models
{
    class User : IUser
    {
        public long? Id { get; }

        public string FirstName { get; }

        public char? MiddleInitial { get; }

        public string LastName { get; }

        public string Email { get; }

        public string Token { get; }

        public User(long? id, string firstName, char? middleInitial, string lastName, string email, string token)
        {
            Id = id;
            FirstName = firstName;
            MiddleInitial = middleInitial;
            LastName = lastName;
            Email = email;
            Token = token;
        }
    }
}
