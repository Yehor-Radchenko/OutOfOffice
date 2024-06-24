using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Newtonsoft.Json;
using Exception = System.Exception;
using OutOfOffice.BLL.Exceptions;

namespace OutOfOffice.Extentions;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    public ProblemDetailsFactory _problemDetailsFactory;

    public GlobalExceptionHandler(RequestDelegate next,
        ProblemDetailsFactory problemDetailsFactory)
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
            case KeyNotFoundException keyNotFoundException:
                problem.Status = (int)HttpStatusCode.NotFound;
                break;
            case ConflictException conflictException:
                problem.Status = (int)HttpStatusCode.Conflict;
                break;
        }

        var problemDetails = new ProblemDetails();

        if (_problemDetailsFactory != null)
        {
            problemDetails = _problemDetailsFactory.CreateProblemDetails(httpContext, problem.Status);

            problem.Title = problemDetails.Title;
            problem.Type = problemDetails.Type;
        }


        var result = new ObjectResult(problem)
        {
            StatusCode = problem.Status
        };

        var response = JsonConvert.SerializeObject(result.Value);
        httpContext.Response.ContentType = "application/problem+json";
        await httpContext.Response.WriteAsync(response);

    }
}
