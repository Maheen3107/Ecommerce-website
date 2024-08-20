namespace Final_Project.Models.User
{
    public interface IUserRepository
    {
        User GetUserByEmailAndPassword(string email, string password);
        User GetUserByEmail(string email);
        bool RegisterUser(User user);
        List<User> GetAllUsers();
    }
}
