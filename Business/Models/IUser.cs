namespace Business.Models
{
    public interface IUser
    {
        long? Id { get; }
        string FirstName { get; }
        char? MiddleInitial { get; }
        string LastName { get; }
        string Email { get; }
        string Token { get; }
    }
}
