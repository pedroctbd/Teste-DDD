using DDDBasico.Application.DTO;
using DDDBasico.Application.Extras;
using DDDBasico.Domain.Entities;
using DDDBasico.Domain.Interfaces;
using FluentValidation;
using MediatR;
using System.Security.Cryptography;
using System.Text;

namespace DDDBasico.Application.Users.Command
{
    public record CreateUserCommand(string UserName, string Password, string Email,string Role = "user") : IRequest<Response>;
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateUserCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(newUser => newUser.UserName).NotEmpty().MaximumLength(100);
            RuleFor(newUser => newUser.Role).NotEmpty();
            RuleFor(newUser => newUser.Email).NotEmpty().MaximumLength(100).EmailAddress();
            RuleFor(newUser => newUser.Password).NotEmpty().MaximumLength(12).MinimumLength(4);
            RuleFor(newUser => newUser).MustAsync(async (newUser, _) => await _unitOfWork.UserRepository.GetByEmailAsync(newUser.Email) == null).WithMessage("Email taken");

        }

    }
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Response>
    {

        private readonly IUnitOfWork _unitOfWork;


        public CreateUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            using var hmac = new HMACSHA512();
            var user = new User
            {
                UserName = request.UserName.ToLower(),
                Email = request.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)),
                PasswordSalt = hmac.Key,
                Role = request.Role
         
            };
  
            
            await _unitOfWork.UserRepository.AddAsync(user);

            await _unitOfWork.SaveChangesAsync();

            return new Response(new UserDTO {Id=user.Id,UserName=user.UserName,Email=user.Email,Role = user.Role });

        }
    }

}


