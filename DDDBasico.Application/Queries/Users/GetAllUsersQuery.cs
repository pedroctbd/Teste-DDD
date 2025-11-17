using DDDBasico.Application.DTO;
using DDDBasico.Application.Extras;
using DDDBasico.Domain.Interfaces;
using MediatR;

namespace DDDBasico.Application.Queries.Users
{
    public record GetAllUsersQuery() : IRequest<Response>;

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Response>
    {

        private readonly IUnitOfWork _unitOfWork;

        public GetAllUsersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task <Response> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.UserRepository.GetAllUsersAsync();
            
            var res = users.Select(user => new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            }); ;
            
            return new Response(users);

        }

    }
}
