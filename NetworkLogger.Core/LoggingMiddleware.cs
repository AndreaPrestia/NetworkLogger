using Microsoft.AspNetCore.Http;
using NetworkLogger.Core.Entities;
using NetworkLogger.Core.Interfaces;

namespace NetworkLogger.Core;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerWriter _loggerWriter;

    public LoggingMiddleware(RequestDelegate next, ILoggerWriter loggerWriter)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _loggerWriter = loggerWriter ?? throw new ArgumentNullException(nameof(loggerWriter));
    }

    public async Task Invoke(HttpContext context)
    {
        var start = DateTime.UtcNow;

        var url = context.Request.Path.Value!;

        var method = context.Request.Method;

        string? errorMessage = null;

        string? responseBody = null;

        string? requestBody = null;

        try
        {
            context.Request.EnableBuffering();

            requestBody = new StreamReader(context.Request.Body).ReadToEndAsync().Result;

            context.Request.Body.Position = 0;

            var originalBodyStream = context.Response.Body;

            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            try
            {
                await _next.Invoke(context).ConfigureAwait(true);
            }
            finally
            {
                responseBody = LoadResponseBody(context.Response);
                responseBodyStream.CopyToAsync(originalBodyStream).GetAwaiter().GetResult();

                context.Response.Body = originalBodyStream;
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"{ex.Message}\n\r{ex.StackTrace}";
        }
        finally
        {
            var ms = (DateTime.UtcNow - start).Milliseconds;

            await _loggerWriter.WriteAsync(new Log()
            {
                Hostname = context.Request.Host.ToString(),
                ClientIp = context.Connection.RemoteIpAddress.ToString(),
                Claims = context.User.Claims.Any() ? context.User.Claims.ToDictionary(x => x.Type, x => x.Value) : null,
                ExecutionTimeMs = ms,
                Url = url,
                Method = method,
                RequestHeaders = context.Request.Headers is { Count: > 0 } ? context.Request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString()) : null,
                ResponseHeaders = context.Response.Headers is { Count: > 0 } ? context.Response.Headers.ToDictionary(x => x.Key, x => x.Value.ToString()) : null,
                QueryString = context.Request.Query is { Count: > 0 }
                    ? context.Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString())
                    : null!,
                Request = requestBody,
                Response = responseBody,
                Error = errorMessage
            });
        }
    }
    
    private static string LoadResponseBody(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);

        var responseText = new StreamReader(response.Body).ReadToEndAsync().Result;

        response.Body.Seek(0, SeekOrigin.Begin);

        return responseText;
    }
}