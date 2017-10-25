namespace Business.Models
{
    public class UserModel
    {
        public int? Id { get; set; }

        public string FirstName { get; set; }

        public char? MiddleInitial { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool IsDeleted { get; set; }
    }
}
