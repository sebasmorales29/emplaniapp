using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    [Table("Estado")]
    public class Estado
    {
        [Key]
        [Column("idEstado")]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string nombreEstado { get; set; }
    }
} 