using NetworkLogger.Core.Entities;

namespace NetworkLogger.Core.Interfaces;

public interface ILoggerHandler
{
    Task Write(Log log);
}