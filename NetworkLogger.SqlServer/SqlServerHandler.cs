using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using NetworkLogger.Core.Entities;
using NetworkLogger.Core.Interfaces;

namespace NetworkLogger.SqlServer;

public class SqlServerHandler : ILoggerHandler
{
    private IConfiguration _configuration;

    public SqlServerHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task Write(Log log)
    {
        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("NetworkStorage")))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("NetworkLogAdd", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@id", Value = log.Id
                });
                command.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@claims", Value = log.Claims.Count > 0 ? JsonSerializer.Serialize(log.Claims) : (object)DBNull.Value
                });
                command.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@created", Value = log.Created
                });
                command.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@hostname", Value = log.Hostname
                });
                command.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@method", Value = log.Method
                });
                command.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@request", Value = !string.IsNullOrWhiteSpace(log.Request) ? log.Request : (object)DBNull.Value
                });
                command.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@response", Value = !string.IsNullOrWhiteSpace(log.Response) ? log.Response : (object)DBNull.Value
                });
                command.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@url", Value = log.Url
                });
                command.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@clientip", Value = log.ClientIp
                });
                command.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@loglevel", Value = log.LogLevel.ToString()
                });
                command.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@querystring", Value = !string.IsNullOrWhiteSpace(log.QueryString) ? log.QueryString : (object)DBNull.Value
                });
                command.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@requestheaders", Value = log.RequestHeaders
                });
                command.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@responseheaders", Value = log.ResponseHeaders
                });
                command.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@executiontimems", Value = log.ExecutionTimeMs
                });

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}