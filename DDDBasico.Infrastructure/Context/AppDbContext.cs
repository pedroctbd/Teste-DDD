using Microsoft.EntityFrameworkCore;
using DDDBasico.Domain.Entities;

namespace DDDBasico.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options){

        }
        
        public DbSet<User>? Users{ get; set; }

    }
}