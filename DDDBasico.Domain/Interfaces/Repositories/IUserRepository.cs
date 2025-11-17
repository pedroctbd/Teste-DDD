using DDDBasico.Domain.Entities;

namespace DDDBasico.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(string id);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task AddAsync(User user);

    }
}