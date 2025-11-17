using DDDBasico.Application.DTO;
using DDDBasico.Application.Extras;
using DDDBasico.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace DDDBasico.Application.Queries.Users
{
    public record GetUserQuery(string Id) : IRequest<Response>;
    public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserQueryValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(newUser => newUser).MustAsync(async (newUser, _) => await _unitOfWork.UserRepository.GetByIdAsync(newUser.Id) == null).WithMessage("User not found");
        }
    }
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<Response> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);

            return new Response(
            new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            });
        }
    }

}
