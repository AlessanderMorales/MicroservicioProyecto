using Microsoft.AspNetCore.Mvc;
using MicroservicioProyecto.Application.Services;
using MicroservicioProyecto.Domain.Entities;

namespace MicroservicioProyecto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProyectoController : ControllerBase
    {
        private readonly ProyectoService _service;
        private readonly ProyectoUsuarioService _usuarioService;

        public ProyectoController(ProyectoService service, ProyectoUsuarioService usuarioService)
        {
            _service = service;
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id) => Ok(_service.GetById(id));

        [HttpPost]
        public IActionResult Create(Proyecto p)
        {
            _service.Add(p);
            return Ok("Proyecto creado.");
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Proyecto p)
        {
            p.IdProyecto = id;
            _service.Update(p);
            return Ok("Proyecto actualizado.");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok("Proyecto eliminado.");
        }

        // Gestión de usuarios
        [HttpGet("{id}/usuarios")]
        public IActionResult GetUsuarios(int id)
            => Ok(_usuarioService.GetUsuarios(id));

        [HttpPost("{id}/usuarios/{usuarioId}")]
        public IActionResult Asignar(int id, int usuarioId)
        {
            _usuarioService.Asignar(id, usuarioId);
            return Ok("Usuario asignado.");
        }

        [HttpDelete("{id}/usuarios/{usuarioId}")]
        public IActionResult Desasignar(int id, int usuarioId)
        {
            _usuarioService.Desasignar(id, usuarioId);
            return Ok("Usuario desasignado.");
        }
    }
}

