using DDDBasico.Application.Middleware;
using DDDBasico.Application.Queries.Users;
using DDDBasico.Application.Users.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDDBasico.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [Authorize]
        [AuthorizeByUserId]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var query = new GetUserQuery(id);
            var request = query with { Id = id };
            var response = await _mediator.Send(request);
            if (response.Errors.Count() != 0) return UnprocessableEntity();
            return Ok(response.Data);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllUsersQuery());
            if (response.Errors.Count() != 0) return UnprocessableEntity();
            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Errors.Count() != 0) return UnprocessableEntity();
            return Ok(response.Data);

        }

    }
}