using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MicroservicioProyecto.Domain.Entities;
using MicroservicioProyecto.Domain.Interfaces;
using MicroservicioProyecto.Infrastructure.Persistence;

namespace MicroservicioProyecto.Infrastructure.Repository
{
    public class ProyectoRepository : IRepository<Proyecto>
    {
        private readonly MySqlConnectionSingleton _connection;


        public ProyectoRepository(MySqlConnectionSingleton connection)
        {
            _connection = connection;
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        public IEnumerable<Proyecto> GetAll()
        {
            using var conn = _connection.CreateConnection();

            return conn.Query<Proyecto>(
                @"SELECT 
                    id_proyecto AS IdProyecto,
                    nombre,
                    descripcion,
                    fecha_inicio AS FechaInicio,
                    fecha_fin AS FechaFin,
                    estado,
                    fechaRegistro,
                    ultimaModificacion
                  FROM Proyecto
                  WHERE estado = 1
                  ORDER BY id_proyecto DESC");
        }

        public Proyecto GetById(int id)
        {
            using var conn = _connection.CreateConnection();

            return conn.QueryFirstOrDefault<Proyecto>(
                @"SELECT 
                    id_proyecto AS IdProyecto,
                    nombre,
                    descripcion,
                    fecha_inicio AS FechaInicio,
                    fecha_fin AS FechaFin,
                    estado,
                    fechaRegistro,
                    ultimaModificacion
                  FROM Proyecto
                  WHERE id_proyecto = @Id;",
                new { Id = id });
        }

        public void Add(Proyecto entity)
        {
            using var conn = _connection.CreateConnection();

            conn.Execute(
                @"INSERT INTO Proyecto (nombre, descripcion, fecha_inicio, fecha_fin)
                  VALUES (@Nombre, @Descripcion, @FechaInicio, @FechaFin);",
                entity);
        }

        public void Update(Proyecto entity)
        {
            using var conn = _connection.CreateConnection();

            conn.Execute(
                @"UPDATE Proyecto
                  SET nombre = @Nombre,
                      descripcion = @Descripcion,
                      fecha_inicio = @FechaInicio,
                      fecha_fin = @FechaFin
                  WHERE id_proyecto = @IdProyecto;",
                entity);
        }

        public void Delete(int id)
        {
            using var conn = _connection.CreateConnection();

            conn.Execute(
                @"UPDATE Proyecto SET estado = 0 WHERE id_proyecto = @Id;",
                new { Id = id });
        }
    }
}

