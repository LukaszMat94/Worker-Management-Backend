using System.Net;
using WorkerManagementAPI.Services.TokenService.Service;

namespace WorkerManagementAPI.Middlewares
{
    public class TokenCheckerMiddleware : IMiddleware
    {
        private readonly ITokenService _tokenService;
        public TokenCheckerMiddleware(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            bool isTokenActive = await _tokenService.IsCurrentAccessTokenActiveAsync();

            if (isTokenActive)
            {
                await next(context);

                return;
            }

            context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
        }
    }
}
