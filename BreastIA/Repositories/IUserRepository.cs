using BreastIA.Models;


namespace BreastIA.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
    }
}
