using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.Entidades;

namespace Emplaniapp.Abstracciones.ModelosAD
{
    [Table("HistorialEmpleado")]
    public class HistorialEmpleado
    {
        [Key]
        [Column("idHistorial")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idHistorial { get; set; }

        [Column("idEmpleado")]
        public int idEmpleado { get; set; }

        [Column("idTipoEvento")]
        public int idTipoEvento { get; set; }

        [Column("fechaEvento")]
        public DateTime fechaEvento { get; set; }

        [Column("descripcionEvento")]
        [Required]
        [StringLength(1000)]
        public string descripcionEvento { get; set; }

        [Column("detallesEvento")]
        public string detallesEvento { get; set; }

        [Column("valorAnterior")]
        public string valorAnterior { get; set; }

        [Column("valorNuevo")]
        public string valorNuevo { get; set; }

        [Column("idUsuarioModificacion")]
        [StringLength(128)]
        public string idUsuarioModificacion { get; set; }

        [Column("ipModificacion")]
        [StringLength(45)]
        public string ipModificacion { get; set; }

        [Column("idEstado")]
        public int idEstado { get; set; }

        [Column("fechaCreacion")]
        public DateTime fechaCreacion { get; set; }

        [Column("fechaModificacion")]
        public DateTime? fechaModificacion { get; set; }

        // Propiedades de navegación
        [ForeignKey("idEmpleado")]
        public virtual Empleados Empleado { get; set; }

        [ForeignKey("idTipoEvento")]
        public virtual TiposEventoHistorial TipoEvento { get; set; }

        [ForeignKey("idEstado")]
        public virtual Estado Estado { get; set; }

        [ForeignKey("idUsuarioModificacion")]
        public virtual ApplicationUser UsuarioModificacion { get; set; }
    }
}
