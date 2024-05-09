using NetworkLogger.Core.Entities;
using NetworkLogger.Core.Interfaces;

namespace NetworkLogger.Test;

public class ConsoleLoggerWriter : ILoggerWriter
{
    public async Task WriteAsync(Log log, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Id : {log.Id}");
        Console.WriteLine($"Created : {log.Created}");
        Console.WriteLine($"Hostname : {log.Hostname}");
        Console.WriteLine($"ClientIp : {log.ClientIp}");

        if (log.Claims != null)
            foreach (var claim in log.Claims)
            {
                Console.WriteLine($"Claim : {claim.Key} {claim.Value}");
            }

        Console.WriteLine($"Url : {log.Url}");
        Console.WriteLine($"Method : {log.Method}");
        if (log.RequestHeaders != null)
            Console.WriteLine($"Request headers : \n\r{string.Join(",", log.RequestHeaders.Select(x => $"{x.Key}:{x.Value}"))}");
        if (log.QueryString != null)
            Console.WriteLine($"Query string : \n\r{string.Join(",", log.QueryString.Select(x => $"{x.Key}:{x.Value}"))}");
        Console.WriteLine($"Request : {log.Request}");
        Console.WriteLine($"Response : {log.Response}");
        if (log.ResponseHeaders != null)
            Console.WriteLine($"Response headers : \n\r{string.Join(",", log.ResponseHeaders.Select(x => $"{x.Key}:{x.Value}"))}");
        Console.WriteLine($"Execution time (ms) : {log.ExecutionTimeMs}");

        //ugly, only for example purposes :) 
        await Task.Delay(10, cancellationToken);
    }
}