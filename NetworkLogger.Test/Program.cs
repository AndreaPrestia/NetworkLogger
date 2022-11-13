using NetworkLogger.Core;
using NetworkLogger.Core.Interfaces;
using NetworkLogger.Test;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ILoggerHandler, ConsoleLoggerHandler>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseMiddleware<LoggingMiddleware>();

app.Run();