using DDDBasico.Application.DTO;
using DDDBasico.Application.Extras;
using DDDBasico.Domain.Interfaces;
using DDDBasico.Domain.Interfaces.Services;
using FluentValidation;
using MediatR;
using System.Security.Cryptography;
using System.Text;

namespace DDDBasico.Application.Command.Auth
{
    public record LoginAuthCommand(string Email,string Password) : IRequest<Response>;
    public class LoginAuthCommandValidator : AbstractValidator<LoginAuthCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoginAuthCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(x => x.Email).MustAsync(async (email, _) =>
                    await _unitOfWork.UserRepository.GetByEmailAsync(email) != null)
                .WithMessage("User not found");
        }

    }
    public class LoginAuthCommandHandler : IRequestHandler<LoginAuthCommand, Response>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public LoginAuthCommandHandler(IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<Response> Handle(LoginAuthCommand request, CancellationToken cancellationToken)
        {

            Response r = new Response();
            var existingUser = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);

            using var hmac = new HMACSHA512(existingUser.PasswordSalt);
            var hashLogin = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

            for (int i = 0; i < hashLogin.Length; i++)
            {
                if (hashLogin[i] != existingUser.PasswordHash[i])
                {
                    r.AddError("Passoword", "Incorrect Password");
                    return r;
                }
            }

            var jwt = new JwtDTO
            {
                Id = existingUser.Id,
                UserName = existingUser.UserName,
                Email = request.Email,
                Token = _tokenService.CreateToken(existingUser),
                Role = existingUser.Role,
                
            };
            return new Response(jwt);


        }

    }

}
