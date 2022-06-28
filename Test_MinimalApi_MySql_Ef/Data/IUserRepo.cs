using MinimalAPI.Models;

namespace MinimalAPI.Data
{
    public interface IUserRepo
    {
        Task SaveChange();
        Task<User?> GetUserById(int id);
        Task<IEnumerable<User>> GetAllUsers();  
        Task CreateUser(User user);

        void DeleteUser(User user);
    }
}
