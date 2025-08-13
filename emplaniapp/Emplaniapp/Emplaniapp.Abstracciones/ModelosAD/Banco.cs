using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    [Table("Bancos")]
    public class Banco
    {
        [Column("idBanco")]
        [Key]
        public int idBanco { get; set; }
        public string nombreBanco { get; set; }
    }
}
