using System;
using AuthenticationService.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthenticationService.Filters
{
    public class TokenAuthenticationFilter : Attribute, IAuthorizationFilter
    {
        //// The filter cannot use the regular way of dependency injection.
        //// Please use (T)context.HttpContext.RequestServices.GetService(typeof(T))
        //public TokenAuthenticationFilter(ITokenManager tokenManager)
        //{
        //    _tokenManager = tokenManager;
        //}

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isAuthorized = true;

            if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                isAuthorized = false;
            }

            var token = string.Empty;

            if (isAuthorized)
            {
                token = context.HttpContext.Request.Headers["Authorization"];
                var tokenManager = (ITokenManager)context.HttpContext.RequestServices.GetService(typeof(ITokenManager));

                try
                {
                    // If the validation will fail it will actually throws an exception.
                    tokenManager.VerifyToken(token);
                }
                catch (Exception ex)
                {
                    isAuthorized = false;
                    context.ModelState.AddModelError("Unauthorized", ex.ToString());
                }
            }

            if (!isAuthorized)
            {
                context.ModelState.AddModelError("Unauthorized", "You're not authorized.");

                // We will skip the rest of the pipeline.
                context.Result = new UnauthorizedObjectResult(context.ModelState);
            }
        }
    }
}   