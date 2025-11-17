using DDDBasico.Application.Middleware;
using DDDBasico.Application.Queries.Users;
using DDDBasico.Application.Users.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // <- add this

namespace DDDBasico.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger; // <- add logger

        public UserController(IMediator mediator, ILogger<UserController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Authorize]
        [AuthorizeByUserId]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            _logger.LogInformation("GET /api/user/{Id} called at {Time}", id, DateTime.UtcNow);

            var query = new GetUserQuery(id);
            var request = query with { Id = id };
            var response = await _mediator.Send(request);

            if (response.Errors.Count() != 0)
            {
                _logger.LogWarning("GET /api/user/{Id} returned errors at {Time}", id, DateTime.UtcNow);
                return UnprocessableEntity();
            }

            _logger.LogInformation("GET /api/user/{Id} succeeded at {Time}", id, DateTime.UtcNow);
            return Ok(response.Data);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GET /api/user called at {Time}", DateTime.UtcNow);

            var response = await _mediator.Send(new GetAllUsersQuery());

            if (response.Errors.Count() != 0)
            {
                _logger.LogWarning("GET /api/user returned errors at {Time}", DateTime.UtcNow);
                return UnprocessableEntity();
            }

            _logger.LogInformation("GET /api/user succeeded at {Time}", DateTime.UtcNow);
            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserCommand command)
        {
            _logger.LogInformation("POST /api/user called at {Time}", DateTime.UtcNow);

            var response = await _mediator.Send(command);

            if (response.Errors.Count() != 0)
            {
                _logger.LogWarning("POST /api/user returned errors at {Time}", DateTime.UtcNow);
                return UnprocessableEntity();
            }

            _logger.LogInformation("POST /api/user succeeded at {Time}", DateTime.UtcNow);
            return Ok(response.Data);
        }
    }
}
