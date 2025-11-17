namespace DDDBasico.Domain.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
    IUserRepository UserRepository { get; }
}
