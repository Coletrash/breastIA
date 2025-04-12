using BreastIA.Models;


namespace BreastIA.Models.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
    }
}
