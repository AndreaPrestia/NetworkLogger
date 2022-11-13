using NetworkLogger.Core.Entities;
using NetworkLogger.Core.Interfaces;

namespace NetworkLogger.Test;

public class ConsoleLoggerHandler : ILoggerHandler
{
    public async Task Write(Log log)
    {
        Console.WriteLine($"Id : {log.Id}");
        Console.WriteLine($"Created : {log.Created}");
        Console.WriteLine($"Level : {log.LogLevel}");
        Console.WriteLine($"Hostname : {log.Hostname}");
        Console.WriteLine($"ClientIp : {log.ClientIp}");

        foreach (var claim in log.Claims)
        {
            Console.WriteLine($"Claim : {claim.Type} {claim.Value}");
        }
         
        Console.WriteLine($"Url : {log.Url}");
        Console.WriteLine($"Method : {log.Method}");
        Console.WriteLine($"Request headers : \n\r{log.RequestHeaders}");
        Console.WriteLine($"Query string : {log.QueryString}");
        Console.WriteLine($"Request : {log.Request}");
        Console.WriteLine($"Response : {log.Response}");
        Console.WriteLine($"Response headers : \n\r{log.ResponseHeaders}");
        Console.WriteLine($"Execution time (ms) : {log.ExecutionTimeMs}");
        
        //ugly, only for example purposes :) 
        await Task.Delay(10);
    }
}