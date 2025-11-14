using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroservicioProyecto.Domain.Entities
{
    public class Proyecto
    {
        public int IdProyecto { get; set; }
        public string Nombre { get; set; } = "";
        public string? Descripcion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime UltimaModificacion { get; set; }
    }
}

