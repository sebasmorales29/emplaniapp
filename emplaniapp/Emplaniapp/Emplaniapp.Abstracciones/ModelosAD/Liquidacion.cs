using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    [Table("Liquidaciones")]
    public class Liquidacion
    {
        [Key]
        public int idLiquidacion { get; set; }
        public int idEmpleado { get; set; }

        // Detalles
        public DateTime fechaLiquidacion { get; set; }
        public string motivoLiquidacion { get; set; }
        public decimal salarioPromedio { get; set; }
        public int aniosAntiguedad { get; set; }
        public int diasPreaviso { get; set; }
        public int diasVacacionesPendientes { get; set; }

        // Cálculo
        public decimal pagoPreaviso { get; set; }
        public decimal pagoAguinaldoProp { get; set; }
        public decimal pagoCesantia { get; set; }
        public decimal pagoVacacionesNG { get; set; }
        public decimal remuPendientes { get; set; }
        public decimal costoLiquidacion { get; set; }

        public string observacionLiquidacion { get; set; }
        public int idEstado { get; set; }
    }
}