using DDDBasico.Application.DTO;
using DDDBasico.Application.Extras;
using DDDBasico.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<GetUserQueryHandler> _logger; 

        public GetUserQueryHandler(IUnitOfWork unitOfWork, ILogger<GetUserQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;

        }


        public async Task<Response> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetUserQuery for Id: {Id} at {Time}", request.Id, DateTime.UtcNow);

            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);

            _logger.LogInformation("User with Id {Id} found. Returning response at {Time}", request.Id, DateTime.UtcNow);

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
