using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    [Table("TipoRetenciones")]
    public class TipoRetencion
    {
        [Column("idTipoRetencion")]
        public int Id { get; set; }

        [Column("nombreTipoRetencio")]
        public string nombreTipoRetencion { get; set; }
        public  double porcentajeRetencion { get; set; }
        public int idEstado { get; set; }

    }
}
