using DDDBasico.Domain.Entities;
using DDDBasico.Domain.Interfaces;
using DDDBasico.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace UserService.Infra.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task<User> GetByIdAsync(string id)
    {
        return await _context.Users.FindAsync(id);
    }
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var res = await _context.Users.FirstOrDefaultAsync(u => u.Email == email.ToLower());
        return res;
    }
}
