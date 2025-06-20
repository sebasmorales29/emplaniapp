using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    [Table("NumeroOcupacion")]
    public class NumeroOcupacion
    {
        [Column("idNumeroOcupacion")]
        [Key]
        public int idNumeroOcupacion { get; set; }
        public int numeroOcupacion { get; set; }

    }
}
