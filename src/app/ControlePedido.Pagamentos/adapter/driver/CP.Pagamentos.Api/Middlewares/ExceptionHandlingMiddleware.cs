using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using CP.Pagamentos.Domain.DomainObjects;
using Microsoft.AspNetCore.Mvc;

namespace CP.Pagamentos.Api.Middlewares;

[ExcludeFromCodeCoverage]
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next,
                                       ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex)
        {
            await HandleExceptionAsync(context, ex, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (ApplicationException ex)
        {
            await HandleExceptionAsync(context, ex, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, "Ocorreu um erro interno no servidor!");
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception, string message, HttpStatusCode status = HttpStatusCode.InternalServerError)
    {
        _logger.LogError(exception, "Ocorreu um erro ao processar a requisição: {Message}. StackTrace: {StackTrace}", exception.Message, exception.StackTrace);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;

        var errorDetails = new ValidationProblemDetails(new Dictionary<string, string[]> {
                {
                    "Mensagens", new string[]{message}
                }
            });

        return context.Response.WriteAsync(JsonSerializer.Serialize(errorDetails));
    }

}
