using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using NetworkLogger.Core.Entities;
using NetworkLogger.Core.Interfaces;

namespace NetworkLogger.SqlServer;

public class SqlServerHandler : ILoggerHandler
{
    private readonly IConfiguration _configuration;

    public SqlServerHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task Write(Log log)
    {
        await using var connection = new SqlConnection(_configuration.GetConnectionString("NetworkStorage"));
        connection.Open();

        await using var command = new SqlCommand("NetworkLogAdd", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add(new SqlParameter()
        {
            ParameterName = "@id", Value = log.Id
        });
        command.Parameters.Add(new SqlParameter()
        {
            ParameterName = "@claims", Value = log.Claims.Count > 0 ? JsonSerializer.Serialize(log.Claims) : DBNull.Value
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
            ParameterName = "@request", Value = !string.IsNullOrWhiteSpace(log.Request) ? log.Request : DBNull.Value
        });
        command.Parameters.Add(new SqlParameter()
        {
            ParameterName = "@response", Value = !string.IsNullOrWhiteSpace(log.Response) ? log.Response : DBNull.Value
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
            ParameterName = "@querystring", Value = !string.IsNullOrWhiteSpace(log.QueryString) ? log.QueryString : DBNull.Value
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