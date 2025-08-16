using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    [Table("Provincia")]
    public class Provincia
    {
        [Column("idProvincia")]
        [Key]
        public int idProvincia { get; set; }
        public string nombreProvincia { get; set; }
    }
}
