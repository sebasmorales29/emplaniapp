using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    [Table("Retencion")]
    public class Retencion
    {
        [Key]
        public int idRetencion { get; set; }
        public int idEmpleado { get; set; }
        public int idTipoRetencion { get; set; }
        public decimal? rebajo { get; set; }
        public DateTime fechaRetencion { get; set; }
        public int idEstado { get; set; }
    }
}
