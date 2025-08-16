using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    [Table("TipoRemuneracion")]
    public class TipoRemuneracion
    {
        [Column("idTipoRemuneracion")]
        public int Id { get; set; }
        public string nombreTipoRemuneracion { get; set; }
        public double porcentajeRemuneracion { get; set; }
        public int idEstado { get; set; }
    }
}
