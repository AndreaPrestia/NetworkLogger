using System.Security.Claims;

namespace NetworkLogger.Core.Entities;

public class Log
{
    public Guid Id { get; set; }
    public List<Claim> Claims { get; set; } = null!;
    public DateTime Created { get; set; }
    public LogLevel LogLevel { get; set; }
    public string ClientIp { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string Method { get; set; } = null!;
    public string RequestHeaders { get; set; } = null!;
    public string ResponseHeaders { get; set; } = null!;

    public string Parameters { get; set; } = null!;
    public string Request { get; set; } = null!;
    public string? Response { get; set; } = null!;
    public int ExecutionTimeMs { get; set; }
}

public enum LogLevel
{
    Info = 'i',
    Debug = 'd',
    Error = 'e',
    Warning = 'w'
}