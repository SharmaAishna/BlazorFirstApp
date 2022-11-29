namespace BlazorClassLibrary.DataAccess;

public interface IUserData
{
    Task CreateUser(UserModel user);
    Task<UserModel> GetUser(string id);
    Task<List<UserModel>> GetUserAsync();
    Task<UserModel> GetUsersFromAuthentication(string objectId);
    Task UpdateUser(UserModel user);
}