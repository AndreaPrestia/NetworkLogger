using NetworkLogger.Core.Entities;

namespace NetworkLogger.Core.Interfaces;

public interface ILoggerWriter
{
    Task WriteAsync(Log log, CancellationToken cancellationToken = default);
}