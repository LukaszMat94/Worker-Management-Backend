﻿using WorkerManagementAPI.Exceptions;

namespace WorkerManagementAPI.Middlewares
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(NotFoundException exception)
            {
                _logger.LogError(exception, exception.Message);
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(exception.Message);
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong");
            }
        }
    }
}
