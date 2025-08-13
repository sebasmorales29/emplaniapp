using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    [Table("Distrito")]
    public class Distrito
    {
        [Column("idDistrito")]
        [Key]
        public int idDistrito { get; set; }
        public string nombreDistrito { get; set; }
        public int idCanton { get; set; }
    }
}
