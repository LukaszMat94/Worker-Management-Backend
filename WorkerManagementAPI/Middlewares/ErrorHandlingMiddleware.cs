using WorkerManagementAPI.Exceptions;
using WorkerManagementAPI.ExceptionsTemplate;

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

                ExceptionDetails exceptionDetails = new ExceptionDetails
                {
                    Message = exception.Message,
                    Status = context.Response.StatusCode = 404,
                    DateTime = DateTime.Now
                };

                await context.Response.WriteAsync(exceptionDetails.ToString());
            }
            catch (DataDuplicateException exception)
            {
                _logger.LogError(exception, exception.Message);

                ExceptionDetails exceptionDetails = new ExceptionDetails
                {
                    Message = exception.Message,
                    Status = context.Response.StatusCode = 409,
                    DateTime = DateTime.Now
                };

                await context.Response.WriteAsync(exceptionDetails.ToString());
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                ExceptionDetails exceptionDetails = new ExceptionDetails
                {
                    Message = "Something went wrong",
                    Status = context.Response.StatusCode = 500,
                    DateTime = DateTime.Now
                };

                await context.Response.WriteAsync(exceptionDetails.ToString());
            }
        }
    }
}
