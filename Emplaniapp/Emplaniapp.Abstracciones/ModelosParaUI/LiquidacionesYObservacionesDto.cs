using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosParaUI
{
    public class LiquidacionesYObservacionesDto
    {
        public int idLiquidacionesYObservaciones { get; set; }
        public int idEmpleado { get; set; }
        public string observacion { get; set; }
        public double? costoLiquidacion { get; set; }
        public string motivoLiquidacion { get; set; }
        public string observacionLiquidacion { get; set; }
        public DateTime fechaObservacion { get; set; }
        public DateTime? fechaLiquidacion { get; set; }
        public int idEstado { get; set; }
        public string nombreEstado { get; set; }
    }
}
