using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    public class Calle
    {
        [Column("idCalle")]
        [Key]
        public int idCalle { get; set; }
        public string nombreCalle { get; set; }
        public int idDistrito { get; set; }
    }
}
