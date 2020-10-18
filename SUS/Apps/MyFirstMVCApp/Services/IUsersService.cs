namespace MyFirstMVCApp.Services
{
    public interface IUsersService
    {
        string CreateUser(string username, string password, string email);

        string GetUserId(string username, string password);

        bool IsUsernameAvailable(string username);

        bool IsEmailAvailable(string email);
    }
}
