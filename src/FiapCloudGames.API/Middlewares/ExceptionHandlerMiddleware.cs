using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGames.API.Middlewares;

internal class ExceptionHandlerMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
		try
		{
            await next(context);
        }
		catch (Exception ex)
		{
            logger.LogError(ex, "Unhandled exception occurred");

            logger.LogInformation("Resposta: {StatusCode}\nCorpo: {Corpo}",
            500,
            string.IsNullOrWhiteSpace(ex.Message + " " + ex.StackTrace));

            context.Response.StatusCode = ex switch
            {
                ApplicationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError,
            };

            await context.Response.WriteAsJsonAsync(
                new ProblemDetails
                {
                    Type = ex.GetType().Name,
                    Title = "An error ocurred",
                    Detail = ex.Message
                });
		}
    }
}
