using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    [Table("Cargos")]
    public class Cargo
    {
        [Key]
        [Column("idCargo")]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string nombreCargo { get; set; }
        
        [Required]
        public int idNumeroOcupacion { get; set; }
    }
} 