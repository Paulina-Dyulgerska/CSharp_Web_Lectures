namespace MyFirstMVCApp.Services
{
    public interface IUserService
    {
        void CreateUser(string username, string password, string email);

        bool IsUserValid(string username, string password);

        bool IsUsernameAvailable(string username);

        bool IsEmailAvailable(string email);
    }
}
