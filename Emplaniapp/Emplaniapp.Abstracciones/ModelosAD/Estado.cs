using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    [Table("Estado")]
    public class Estado
    {
        [Column("idEstado")]
        [Key]
        public int idEstado { get; set; }
        public string nombreEstado { get; set; }
    }
}
