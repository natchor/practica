using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

using Microsoft.Extensions.Configuration;

public class DatabaseHelper
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;

    public DatabaseHelper(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public List<Antecedentes> GetFichaMedicaData()
    {
        return _context.Antecedentes.ToList();
    }

    public string PingDatabase()
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return "Conexión exitosa a la base de datos.";
            }
        }
        catch (Exception ex)
        {
            return $"Error al conectar con la base de datos: {ex.Message}";
        }
    }
}