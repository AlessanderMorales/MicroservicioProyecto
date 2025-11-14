using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MicroservicioProyecto.Domain.Entities;
using MicroservicioProyecto.Infrastructure.Persistence;

namespace MicroservicioProyecto.Infrastructure.Repository
{
    public class ProyectoUsuarioRepository
    {
        private readonly MySqlConnectionSingleton _connection;

        public ProyectoUsuarioRepository(MySqlConnectionSingleton connection)
        {
            _connection = connection;
        }

        public IEnumerable<ProyectoUsuario> GetUsuariosAsignados(int idProyecto)
        {
            using var conn = _connection.CreateConnection();

            return conn.Query<ProyectoUsuario>(
                @"SELECT id, id_proyecto AS IdProyecto, id_usuario AS IdUsuario,
                         fecha_asignacion AS FechaAsignacion, estado
                  FROM Proyecto_Usuario
                  WHERE id_proyecto = @IdProyecto AND estado = 1
                  ORDER BY fecha_asignacion;",
                new { IdProyecto = idProyecto });
        }

        public void Asignar(int idProyecto, int idUsuario)
        {
            using var conn = _connection.CreateConnection();

            conn.Execute(
                @"CALL sp_asignar_usuario_proyecto(@IdProyecto, @IdUsuario);",
                new { IdProyecto = idProyecto, IdUsuario = idUsuario });
        }

        public void Desasignar(int idProyecto, int idUsuario)
        {
            using var conn = _connection.CreateConnection();

            conn.Execute(
                @"CALL sp_desasignar_usuario_proyecto(@IdProyecto, @IdUsuario);",
                new { IdProyecto = idProyecto, IdUsuario = idUsuario });
        }
    }
}

