using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    [Table("Canton")]
    public class Canton
    {
        [Column("idCanton")]
        [Key]
        public int idCanton { get; set; }
        public string nombreCanton { get; set; }
        public int idProvincia { get; set; }

    }
}
