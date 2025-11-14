using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace MicroservicioProyecto.Infrastructure.Persistence
{
    public class MySqlConnectionSingleton
    {
        private readonly string _connectionString;

        public MySqlConnectionSingleton(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ProyectoDbConnection");
        }

        public MySqlConnection CreateConnection()
            => new MySqlConnection(_connectionString);
    }
}

