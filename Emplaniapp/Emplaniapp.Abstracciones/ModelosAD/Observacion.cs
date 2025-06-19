using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Emplaniapp.Abstracciones.ModelosAD;

namespace Emplaniapp.Abstracciones.Entidades
{
    [Table("Observaciones")]
    public class Observacion
    {
        [Key]
        [Column("idObservacion")]
        public int IdObservacion { get; set; }

        [Required]
        [Column("idEmpleado")]
        public int IdEmpleado { get; set; }

        [Required]
        [Column("titulo")]
        [MaxLength(200)]
        public string Titulo { get; set; }

        [Required]
        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Required]
        [Column("idUsuarioCreo")]
        public string IdUsuarioCreo { get; set; }

        [Required]
        [Column("fechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        [Column("idUsuarioEdito")]
        public string IdUsuarioEdito { get; set; }

        [Column("fechaEdicion")]
        public DateTime? FechaEdicion { get; set; }

        // Navigation Properties
        [ForeignKey("IdEmpleado")]
        public virtual Empleados Empleado { get; set; }
    }
} 