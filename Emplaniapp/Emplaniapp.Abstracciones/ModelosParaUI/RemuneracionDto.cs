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
        public int? horasTrabajadas { get; set; }
        public int? horasExtras { get; set; }
        public double? comision { get; set; }
        public double? pagoQuincenal { get; set; }
        public double? horasFeriados { get; set; }
        public double? horasVacaciones { get; set; }
        public double? horasLicencias { get; set; }
        public int idEstado { get; set; }
        public string nombreEstado { get; set; }
    }
}
