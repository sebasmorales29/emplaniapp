using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosParaUI
{
    public class RemuneracionDto
    {
        public int idRemuneracion { get; set; }
        public int idEmpleado { get; set; }
        public int idTipoRemuneracion { get; set; }
        public string nombreTipoRemuneracion { get; set; }
        public DateTime fechaRemuneracion { get; set; }
        public int? diasTrabajados { get; set; }
        public int? horas { get; set; }
        public decimal? comision { get; set; }
        public decimal? pagoQuincenal { get; set; }
        public int idEstado { get; set; }
        public string nombreEstado { get; set; }
        public double? porcentajeRemuneracion { get; set; }
    }
}
