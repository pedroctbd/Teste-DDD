using DDDBasico.Application.DTO;
using DDDBasico.Application.Extras;
using DDDBasico.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging; // <- add this

namespace DDDBasico.Application.Queries.Users
{
    public record GetAllUsersQuery() : IRequest<Response>;

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllUsersQueryHandler> _logger; 

        public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllUsersQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Response> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllUsersQuery at {Time}", DateTime.UtcNow);

            var users = await _unitOfWork.UserRepository.GetAllUsersAsync();

            if (users == null || !users.Any())
            {
                _logger.LogWarning("No users found at {Time}", DateTime.UtcNow);
            }
            else
            {
                _logger.LogInformation("{Count} users found. Returning response at {Time}", users.Count(), DateTime.UtcNow);
            }

            var res = users.Select(user => new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            });

            return new Response(users);
        }
    }
}
