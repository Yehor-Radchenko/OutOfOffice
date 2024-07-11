using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Newtonsoft.Json;
using Exception = System.Exception;
using OutOfOffice.Common.Exceptions;

namespace OutOfOffice.Extentions
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ProblemDetailsFactory _problemDetailsFactory;

        public GlobalExceptionHandler(RequestDelegate next, ProblemDetailsFactory problemDetailsFactory)
        {
            _next = next;
            _problemDetailsFactory = problemDetailsFactory;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandleAsync(httpContext, e);
            }
        }

        public async Task HandleAsync(HttpContext httpContext, Exception exception)
        {
            var problem = new ProblemDetails
            {
                Instance = httpContext.Request.Path,
                Status = (int)HttpStatusCode.InternalServerError,
                Detail = exception.Message
            };

            switch (exception)
            {
                case KeyNotFoundException _:
                    problem.Status = (int)HttpStatusCode.NotFound;
                    break;
                case ConflictException _:
                    problem.Status = (int)HttpStatusCode.Conflict;
                    break;
                case UnauthorizedAccessException _:
                    problem.Status = (int)HttpStatusCode.Unauthorized;
                    break;
            }

            if (_problemDetailsFactory != null)
            {
                var problemDetails = _problemDetailsFactory.CreateProblemDetails(httpContext, problem.Status);
                problem.Title = problemDetails.Title;
                problem.Type = problemDetails.Type;
            }

            var response = JsonConvert.SerializeObject(problem);
            httpContext.Response.ContentType = "application/problem+json";
            httpContext.Response.StatusCode = problem.Status ?? (int)HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsync(response);
        }
    }
}
