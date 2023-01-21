using Microsoft.AspNetCore.Http;
using NetworkLogger.Core.Entities;
using NetworkLogger.Core.Interfaces;
using LogLevel = NetworkLogger.Core.Entities.LogLevel;

namespace NetworkLogger.Core;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerHandler _loggerHandler;

    public LoggingMiddleware(RequestDelegate next, ILoggerHandler loggerHandler)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _loggerHandler = loggerHandler ?? throw new ArgumentNullException(nameof(loggerHandler));
    }

    public async Task Invoke(HttpContext context)
    {
        var start = DateTime.UtcNow;

        var url = context.Request.Path.Value!;

        var method = context.Request.Method;

        var hasError = false;

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
            hasError = true;

            responseBody = $"{ex.Message}\n\r{ex.StackTrace}";
        }
        finally
        {
            var ms = (DateTime.UtcNow - start).Milliseconds;

            await _loggerHandler.Write(new Log()
            {
                Hostname = context.Request.Host.ToString(),
                ClientIp = context.Connection.RemoteIpAddress.ToString(),
                Claims = context.User.Claims.ToList(),
                ExecutionTimeMs = ms,
                LogLevel = hasError ? LogLevel.Error : LogLevel.Info,
                Url = url,
                Method = method,
                RequestHeaders = string.Join("\n\r", context.Request.Headers.Select(x => $"{x.Key}={x.Value}")),
                ResponseHeaders = string.Join("\n\r", context.Response.Headers.Select(x => $"{x.Key}={x.Value}")),
                QueryString = !string.IsNullOrEmpty(context.Request.QueryString.Value)
                    ? context.Request.QueryString.Value!
                    : null!,
                Request = requestBody!,
                Response = responseBody
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