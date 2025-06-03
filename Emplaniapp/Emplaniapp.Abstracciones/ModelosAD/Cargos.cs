using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    [Table("Cargos")]
    public class Cargos
    {
        [Column("idCargo")]
        [Key]
        public int idCargo { get; set; }
        public string nombreCargo { get; set; }
        public int idNumeroOcupacion { get; set; }
    }
}
