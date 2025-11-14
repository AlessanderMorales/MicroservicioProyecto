using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroservicioProyecto.Domain.Entities;
using MicroservicioProyecto.Domain.Interfaces;

namespace MicroservicioProyecto.Application.Services
{
    public class ProyectoService
    {
        private readonly IRepository<Proyecto> _repo;

        public ProyectoService(IRepository<Proyecto> repo)
        {
            _repo = repo;
        }

        public IEnumerable<Proyecto> GetAll() => _repo.GetAll();
        public Proyecto GetById(int id) => _repo.GetById(id);
        public void Add(Proyecto p) => _repo.Add(p);
        public void Update(Proyecto p) => _repo.Update(p);
        public void Delete(int id) => _repo.Delete(id);
    }
}

