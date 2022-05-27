using System.Net;
using WorkerManagementAPI.Services.TokenService.Repository;

namespace WorkerManagementAPI.Middlewares
{
    public class TokenCheckerMiddleware : IMiddleware
    {
        private readonly ITokenManager _tokenManager;
        public TokenCheckerMiddleware(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            bool isTokenActive = await _tokenManager.IsCurrentAccessTokenActiveAsync();

            if (isTokenActive)
            {
                await next(context);

                return;
            }

            context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
        }
    }
}
