using System.Net;
using Domain.Exceptions;
using Shared.Error_Models;

namespace E_commerce
{
    public class CustomExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandleMiddleware> _logger;

        public CustomExceptionHandleMiddleware(RequestDelegate Next ,ILogger<CustomExceptionHandleMiddleware> logger)
        {
            _next = Next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
                if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    var Response = new ErrorToReturn()
                    {
                        StatusCode = httpContext.Response.StatusCode,
                        ErrorMessage = $"End Point {httpContext.Request.Path} is Not Found"
                    };
                  await httpContext.Response.WriteAsJsonAsync(Response);
                }

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);


                httpContext.Response.StatusCode = ex switch

                {
                    NotFoundException => StatusCodes.Status404NotFound,

                    _ => StatusCodes.Status500InternalServerError
                };
                //set status code for response
                //httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //set content type for response
                httpContext.Response.ContentType = "application/json";
                //response object
                var Response = new ErrorToReturn()
                {
                    StatusCode = httpContext.Response.StatusCode,
                    ErrorMessage = ex.Message
                };
                //return object as json
                await httpContext.Response.WriteAsJsonAsync(Response);
            }
        }
    }
}
