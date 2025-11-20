using Microsoft.AspNetCore.Mvc;
using MicroservicioProyecto.Application.Services;
using MicroservicioProyecto.Domain.Entities;
using MicroservicioProyecto.Domain.Validators;
using Microsoft.Extensions.Logging;

namespace MicroservicioProyecto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProyectoController : ControllerBase
    {
        private readonly ProyectoService _service;
        private readonly ProyectoUsuarioService _usuarioService;
        private readonly ILogger<ProyectoController> _logger;

        public ProyectoController(ProyectoService service, ProyectoUsuarioService usuarioService, ILogger<ProyectoController> logger)
        {
            _service = service;
            _usuarioService = usuarioService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var lista = _service.GetAll().ToList();
            return Ok(lista);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new { error = true, message = "El ID debe ser mayor a 0." });

            var proyecto = _service.GetById(id);
            if (proyecto == null)
                return NotFound(new { error = true, message = "Proyecto no encontrado" });

            return Ok(proyecto);
        }

        [HttpPost]
        public IActionResult Create(Proyecto p)
        {
            try
            {
                if (p == null)
                    return BadRequest(new { error = true, message = "Los datos del proyecto son requeridos." });

                p.Nombre = InputValidator.ValidateAndSanitize(p.Nombre, "Nombre");

                if (!string.IsNullOrWhiteSpace(p.Descripcion))
                    p.Descripcion = InputValidator.SanitizeText(p.Descripcion);

                if (p.FechaInicio.HasValue)
                    p.FechaInicio = InputValidator.ValidateDate(p.FechaInicio.Value, canBePast: false, canBeFuture: true);

                if (p.FechaFin.HasValue)
                    p.FechaFin = InputValidator.ValidateDate(p.FechaFin.Value, canBePast: false, canBeFuture: true);

                InputValidator.ValidateDateRange(p.FechaInicio, p.FechaFin);

                p.Estado = 1;
                p.FechaRegistro = DateTime.Now;
                p.UltimaModificacion = DateTime.Now;

                _service.Add(p);
                _logger.LogInformation($"Proyecto creado: {p.Nombre}");

                return Ok(new { error = false, message = "Proyecto creado.", data = p });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"Validación fallida al crear proyecto: {ex.Message}");
                return BadRequest(new { error = true, message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, Proyecto p)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { error = true, message = "El ID debe ser mayor a 0." });

                if (p == null)
                    return BadRequest(new { error = true, message = "Los datos del proyecto son requeridos." });

                p.Nombre = InputValidator.ValidateAndSanitize(p.Nombre, "Nombre");

                if (!string.IsNullOrWhiteSpace(p.Descripcion))
                    p.Descripcion = InputValidator.SanitizeText(p.Descripcion);

                if (p.FechaInicio.HasValue)
                    p.FechaInicio = InputValidator.ValidateDate(p.FechaInicio.Value, canBePast: false, canBeFuture: true);

                if (p.FechaFin.HasValue)
                    p.FechaFin = InputValidator.ValidateDate(p.FechaFin.Value, canBePast: false, canBeFuture: true);

                InputValidator.ValidateDateRange(p.FechaInicio, p.FechaFin);

                p.IdProyecto = id;
                p.UltimaModificacion = DateTime.Now;

                _service.Update(p);
                _logger.LogInformation($"Proyecto actualizado: {id}");

                return Ok(new { error = false, message = "Proyecto actualizado." });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"Validación fallida al actualizar proyecto: {ex.Message}");
                return BadRequest(new { error = true, message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { error = true, message = "El ID debe ser mayor a 0." });

            var proyecto = _service.GetById(id);
            if (proyecto == null)
                return NotFound(new { error = true, message = "Proyecto no encontrado" });

            _service.Delete(id);
            _logger.LogInformation($"Proyecto eliminado: {id}");

            return Ok(new { error = false, message = "Proyecto eliminado." });
        }

        [HttpGet("{idProyecto:int}/usuarios")]
        public IActionResult GetUsuarios(int idProyecto)
        {
            if (idProyecto <= 0)
                return BadRequest(new { error = true, message = "El ID del proyecto debe ser mayor a 0." });

            return Ok(_usuarioService.GetUsuarios(idProyecto));
        }

        [HttpPost("{idProyecto:int}/usuarios/{idUsuario:int}")]
        public IActionResult Asignar(int idProyecto, int idUsuario)
        {
            if (idProyecto <= 0 || idUsuario <= 0)
                return BadRequest(new { error = true, message = "Los IDs deben ser mayores a 0." });

            _usuarioService.Asignar(idProyecto, idUsuario);
            _logger.LogInformation($"Usuario {idUsuario} asignado al proyecto {idProyecto}");

            return Ok(new { error = false, message = "Usuario asignado." });
        }

        [HttpDelete("{idProyecto:int}/usuarios/{idUsuario:int}")]
        public IActionResult Desasignar(int idProyecto, int idUsuario)
        {
            if (idProyecto <= 0 || idUsuario <= 0)
                return BadRequest(new { error = true, message = "Los IDs deben ser mayores a 0." });

            _usuarioService.Desasignar(idProyecto, idUsuario);
            _logger.LogInformation($"Usuario {idUsuario} desasignado del proyecto {idProyecto}");

            return Ok(new { error = false, message = "Usuario desasignado." });
        }

        [HttpGet("por-usuario/{idUsuario:int}")]
        public IActionResult GetProyectosByUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                return BadRequest(new { error = true, message = "El ID del usuario debe ser mayor a 0." });

            var proyectos = _usuarioService.GetProyectosByUsuario(idUsuario);
            return Ok(proyectos);
        }
    }
}
