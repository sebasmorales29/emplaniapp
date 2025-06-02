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
    public class Liquidaciones
    {
        [Column("idLiquidacion")]
        [Key]
        public int idLiquidacion { get; set; }
        public int idEmpleado { get; set; }
        public decimal costoLiquidacion { get; set; }
        public string motivoLiquidacion { get; set; }
        public string observacionLiquidacion { get; set; }
        public DateTime fechaLiquidacion { get; set; }
        public int idEstado { get; set; }
    }
}
