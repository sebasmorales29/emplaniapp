using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    [Table("TipoMoneda")]
    public class TipoMoneda
    {
        [Key]
        [Column("idTipoMoneda")]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string nombreMoneda { get; set; }
    }
} 