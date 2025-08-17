using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    [Table("TiposEventoHistorial")]
    public class TiposEventoHistorial
    {
        [Key]
        [Column("idTipoEvento")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idTipoEvento { get; set; }

        [Column("nombreEvento")]
        [Required]
        [StringLength(100)]
        public string nombreEvento { get; set; }

        [Column("descripcionEvento")]
        [StringLength(500)]
        public string descripcionEvento { get; set; }

        [Column("categoriaEvento")]
        [Required]
        [StringLength(50)]
        public string categoriaEvento { get; set; }

        [Column("iconoEvento")]
        [StringLength(50)]
        public string iconoEvento { get; set; }

        [Column("colorEvento")]
        [StringLength(20)]
        public string colorEvento { get; set; }

        [Column("idEstado")]
        public int idEstado { get; set; }

        [Column("fechaCreacion")]
        public DateTime fechaCreacion { get; set; }

        [Column("fechaModificacion")]
        public DateTime? fechaModificacion { get; set; }

        // Propiedades de navegación
        [ForeignKey("idEstado")]
        public virtual Estado Estado { get; set; }
    }
}
