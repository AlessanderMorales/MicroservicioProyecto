using MicroservicioProyecto.Domain.Entities;
using MicroservicioProyecto.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroservicioProyecto.Application.Services
{
    public class ProyectoUsuarioService
    {
        private readonly ProyectoUsuarioRepository _repo;

        public ProyectoUsuarioService(ProyectoUsuarioRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<dynamic> GetUsuarios(int idProyecto)
            => _repo.GetUsuariosAsignados(idProyecto);

        public void Asignar(int idProyecto, int idUsuario)
            => _repo.Asignar(idProyecto, idUsuario);

        public void Desasignar(int idProyecto, int idUsuario)
            => _repo.Desasignar(idProyecto, idUsuario);

        public IEnumerable<Proyecto> GetProyectosByUsuario(int idUsuario)
        {
            return _repo.GetProyectosByUsuario(idUsuario);
        }

    }
}

