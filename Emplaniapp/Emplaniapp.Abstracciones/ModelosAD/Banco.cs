using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    [Table("Bancos")]
    public class Banco
    {
        [Key]
        [Column("idBanco")]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string nombreBanco { get; set; }
    }
} 