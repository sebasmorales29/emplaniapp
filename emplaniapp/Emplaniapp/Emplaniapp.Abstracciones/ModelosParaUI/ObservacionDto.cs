using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Emplaniapp.Abstracciones.ModelosParaUI
{
    public class ObservacionDto
    {
        public int IdObservacion { get; set; }
        public int IdEmpleado { get; set; }

        [DisplayName("Título")]
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(200, ErrorMessage = "El título no puede tener más de 200 caracteres.")]
        public string Titulo { get; set; }

        [DisplayName("Descripción")]
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; }

        public string IdUsuarioCreo { get; set; }
        public string NombreUsuarioCreo { get; set; }
        public DateTime FechaCreacion { get; set; }

        public string IdUsuarioEdito { get; set; }
        public string NombreUsuarioEdito { get; set; }
        public DateTime? FechaEdicion { get; set; }
    }
} 