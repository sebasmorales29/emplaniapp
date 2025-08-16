using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    [Table("TipoMoneda")]
    public class TipoMoneda
    {
        [Column("idTipoMoneda")]
        [Key]
        public int idTipoMoneda { get; set; }
        public string nombreMoneda { get; set; }
    }
}
