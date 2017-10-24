namespace Models.Models
{
    public class User : IUser
    {
        public long? Id { get; set; }

        public string FirstName { get; set; }

        public char? MiddleInitial { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
