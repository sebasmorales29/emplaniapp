using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    [Table("PeridoPago")]
    public class PeriodoPagos
    { 
        [Column("idPeriodoPago")]
        [Key]
        public int idPeriodoPago { get; set; }
        public string periodoPago { get; set; }
        public bool aprobacion { get; set; }
        public DateTime fechaAprobado { get; set; }
        public string idUsuario { get; set; }
        public string registroPeriodoPago { get; set; }
        public DateTime? fechaCreacion { get; set; }
    }
}
