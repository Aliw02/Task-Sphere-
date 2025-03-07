using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.IO;
using System.Reflection;

namespace DAL;

public class ConnectionToTheDB
{
    private readonly string? _connectionString;

    public ConnectionToTheDB()
    {

        string path = Path.Combine(Directory.GetCurrentDirectory(), "Config");

        var config = new ConfigurationBuilder()
            .SetBasePath(path) // Ensure the correct path
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        _connectionString = config.GetConnectionString("TaskSphereDB");

        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new Exception("Database connection string is missing. Check appsettings.json.");
        }
    }

    public SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public string? GetConnectionString()
    {
        return _connectionString;
    }
}