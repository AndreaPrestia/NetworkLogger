# NetworkLogger
A network logger that writes every operations that occurs on AspNet WebApi/MinimalApi application. 

It can work writing data logged on every storage that you want (SQL, NoSQL, File, etc...).

It logs every operation on the pipeline.

It's already tested in a real production environment :) 

How to use it? 

You can follow the example in the directory Example where is located a project NetworkLogger.Test (an empty minimal api AspNet web application).

In the Program.cs there is the configuration part (to be refined in an extension) and works as is:

- builder.Services.AddSingleton<ILoggerHandler, ConsoleLoggerHandler>(); (Registration of the ILoggerHandler implementation (currently a console logger one))

- app.UseMiddleware<LoggingMiddleware>(); (Registration of the logging middleware to use).

That's it! Just use your favorite storage/presentation system and implement your own ILoggerHandler :) .

Soon i will implement the SQLServer, CosmosDb, Oracle, MySql, MongoDb handlers. 




