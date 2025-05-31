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
        [Column("idTipoRetenciones")]
        public int Id { get; set; }
        public string nombreTipoRetencion { get; set; }
        public int porcentajeRetencion { get; set; }
        public int idEstado { get; set; }

    }
}
