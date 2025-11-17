using DDDBasico.Application.Middleware;
using DDDBasico.Domain.Interfaces;
using DDDBasico.Domain.Interfaces.Services;
using DDDBasico.Infrastructure;
using DDDBasico.Infrastructure.Context;
using DDDBasico.Infrastructure.Services.JWT;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public static class DepencyInjection
{
    public static void ConfigureStartup(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();

        var assembly = AppDomain.CurrentDomain.Load("DDDBasico.Application");
        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(assembly));
        services.AddMediatR(assembly);
        ConfigureDB(services, config);
        ConfigureAuth(services, config);
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddHttpContextAccessor();
    }


    public static void ConfigureDB(this IServiceCollection services, IConfiguration config)
    {

        services.AddDbContext<AppDbContext>(options =>{options.UseSqlite(config.GetConnectionString("DefaultConnection"),b=>b.MigrationsAssembly("DDDBasico.Infrastructure"));});

    }


    private static void ConfigureAuth(IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]!)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        services.AddAuthorization();
    }

}
