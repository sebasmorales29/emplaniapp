using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    [Table("Remuneracion")]
    public class Remuneracion
    {
        [Column("idRemuneracion")]
        [Key]
        public int idRemuneracion { get; set; }
        public int idEmpleado { get; set; }
        public int idTipoRemuneracion { get; set; }
        public DateTime fechaRemuneracion { get; set; }
        public int? diasTrabajados { get; set; }
        public int? horas { get; set; }
        public decimal? comision { get; set; }
        public decimal? pagoQuincenal { get; set; }
        public int idEstado { get; set; }

        [ForeignKey("idTipoRemuneracion")]
        public virtual TipoRemuneracion TipoRemuneracion { get; set; }

        [ForeignKey("idEstado")]
        public virtual Estado Estado { get; set; }

        [ForeignKey("idEmpleado")]
        public virtual Empleados Empleado { get; set; }
    }
}
