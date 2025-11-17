using DDDBasico.Domain.Entities;

namespace DDDBasico.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
        string ReturnIdToken(string token);
    }
}
