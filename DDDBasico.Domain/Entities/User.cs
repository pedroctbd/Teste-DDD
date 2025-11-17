using Swashbuckle.AspNetCore.Annotations;
using System.Data;

namespace DDDBasico.Domain.Entities
{
    public class User
    {
        public string Id{ get; set; } = Guid.NewGuid().ToString();
        public string? UserName { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public byte[]? PasswordHash { get; set; }
        public string? Email { get; set; }
        public string Role { get; set; } = "user"; 

    }
}