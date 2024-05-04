namespace NetworkLogger.Core.Entities;

public class Log
{
    public Log()
    {
        Id = Guid.NewGuid();
        Created = DateTime.UtcNow;
    }
    
    public Guid Id { get; set; }
    public Dictionary<string, string>? Claims { get; set; }
    public DateTime Created { get; set; }
    public string ClientIp { get; set; } = null!;
    public string Hostname { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string Method { get; set; } = null!;
    public Dictionary<string, string>? RequestHeaders { get; set; }
    public Dictionary<string, string>? ResponseHeaders { get; set; }
    public Dictionary<string, string>? QueryString { get; set; }
    public string? Request { get; set; }
    public string? Response { get; set; }
    public string? Error { get; set; }
    public int ExecutionTimeMs { get; set; }
}