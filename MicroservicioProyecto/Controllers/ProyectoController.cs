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

        // ============================================
        // PROYECTOS CRUD
        // ============================================
        [HttpGet]
        public IActionResult GetAll()
        {
            var lista = _service.GetAll().ToList();
            return Ok(lista);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var proyecto = _service.GetById(id);

            if (proyecto == null)
                return NotFound(new { mensaje = "Proyecto no encontrado" });

            return Ok(proyecto);
        }


        [HttpPost]
        public IActionResult Create(Proyecto p)
        {
            _service.Add(p);
            return Ok("Proyecto creado.");
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, Proyecto p)
        {
            p.IdProyecto = id;
            _service.Update(p);
            return Ok("Proyecto actualizado.");
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok("Proyecto eliminado.");
        }

        // ============================================
        // USUARIOS ASIGNADOS A PROYECTO
        // ============================================
        [HttpGet("{idProyecto:int}/usuarios")]
        public IActionResult GetUsuarios(int idProyecto)
            => Ok(_usuarioService.GetUsuarios(idProyecto));

        [HttpPost("{idProyecto:int}/usuarios/{idUsuario:int}")]
        public IActionResult Asignar(int idProyecto, int idUsuario)
        {
            _usuarioService.Asignar(idProyecto, idUsuario);
            return Ok("Usuario asignado.");
        }

        [HttpDelete("{idProyecto:int}/usuarios/{idUsuario:int}")]
        public IActionResult Desasignar(int idProyecto, int idUsuario)
        {
            _usuarioService.Desasignar(idProyecto, idUsuario);
            return Ok("Usuario desasignado.");
        }

        // ============================================
        // PROYECTOS POR USUARIO  (RUTA CORREGIDA)
        // ============================================
        // ANTES: "usuario/{idUsuario}"  ← conflicto con {id}
        // AHORA: "por-usuario/{idUsuario}"
        [HttpGet("por-usuario/{idUsuario:int}")]
        public IActionResult GetProyectosByUsuario(int idUsuario)
        {
            var proyectos = _usuarioService.GetProyectosByUsuario(idUsuario);
            return Ok(proyectos);
        }


    }
}
