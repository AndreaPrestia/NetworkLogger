﻿using System.Security.Claims;

namespace NetworkLogger.Core.Entities;

public class Log
{
    public Log()
    {
        Id = Guid.NewGuid();
        Created = DateTime.UtcNow;
        Claims = new List<Claim>();
    }
    
    public Guid Id { get; set; }
    public List<Claim> Claims { get; set; }
    public DateTime Created { get; set; }
    public LogLevel LogLevel { get; set; }
    public string ClientIp { get; set; } = null!;
    public string Hostname { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string Method { get; set; } = null!;
    public string RequestHeaders { get; set; } = null!;
    public string ResponseHeaders { get; set; } = null!;

    public string QueryString { get; set; } = null!;
    public string Request { get; set; } = null!;
    public string? Response { get; set; }
    public int ExecutionTimeMs { get; set; }
}

public enum LogLevel
{
    Info = 'i',
    Debug = 'd',
    Error = 'e',
    Warning = 'w'
}