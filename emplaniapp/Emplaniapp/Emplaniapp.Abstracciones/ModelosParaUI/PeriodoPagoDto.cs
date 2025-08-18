using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosParaUI
{
    public class PeriodoPagoDto
    {
        public int idPeriodoPago { get; set; }
        public int PeriodoPago { get; set; }
        public bool aprobacion { get; set; }
        public DateTime? fechaAprobado { get; set; }
        public string idUsuario { get; set; }
        public string registroPeriodoPago { get; set; }
        public DateTime? fechaCreacion { get; set; }

    }
}
