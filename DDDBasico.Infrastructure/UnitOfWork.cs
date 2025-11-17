using DDDBasico.Domain.Interfaces;
using DDDBasico.Infrastructure.Context;
using UserService.Infra.Data.Repositories;

namespace DDDBasico.Infrastructure;

public class UnitOfWork : IUnitOfWork
{

    private AppDbContext _context;
    private IUserRepository _userRepository;


    public UnitOfWork(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}

