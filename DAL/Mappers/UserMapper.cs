using Business.Models;
using DAL.Models;

namespace DAL.Mappers
{
    class UserMapper : IUser
    {
        private readonly User user;

        public UserMapper(User user)
        {
            this.user = user;
        }

        public long? Id => user.UserId;

        public string FirstName => user.FirstName;

        public char? MiddleInitial => user.MiddleInitial[0];

        public string LastName => user.LastName;

        public string Email => user.Email;
    }
}
