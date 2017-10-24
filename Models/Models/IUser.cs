namespace Models.Models
{
    public interface IUser
    {
        long? Id { get; set; }
        string FirstName { get; set; }
        char? MiddleInitial { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
    }
}
