using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    [Table("PagoQuincenal")]
    public class PagoQuincenal
    {
        [Column("idPagoQuincenal")]
        [Key]
        public int idPagoQuincenal { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public int idPeriodoPago { get; set; }
        public int idEmpleado { get; set; }
        public int? idRemuneracion { get; set; }
        public int? idRetencion { get; set; }
        public decimal salarioNeto { get; set; }
        public int? idLiquidacion { get; set; }
        public bool aprobacion { get; set; }
        public string idUsuario { get; set; }
    }
}
