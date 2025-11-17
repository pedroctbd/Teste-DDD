using DDDBasico.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace DDDBasico.Application.Middleware
{
    public class AuthorizeByUserIdAttribute : ActionFilterAttribute
    {

        public AuthorizeByUserIdAttribute()
        {
       
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            var user = context.HttpContext.User;
            if (user.IsInRole("admin"))
            {
                return; 
            }

            var tokenService = context.HttpContext.RequestServices.GetService<ITokenService>();
            var userIdFromToken = tokenService.ReturnIdToken(context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));

            foreach (var arg in context.ActionArguments.Values)
            {
                var userIdFromBody = Guid.Parse(arg.ToString());
                if (userIdFromBody.ToString() != userIdFromToken)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                } 
            }
        }
    }



}
