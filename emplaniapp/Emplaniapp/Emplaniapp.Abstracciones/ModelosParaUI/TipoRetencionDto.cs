using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosParaUI
{
    public class TipoRetencionDto
    {
        public int Id { get; set; }

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El nombre de la retención es obligatorio.")]
        public string nombreTipoRetencion { get; set; }

        [DisplayName("Porcentaje %")]
        [Required(ErrorMessage = "El porcentaje de retención es obligatorio.")]
        public double porcentajeRetencion { get; set; }

        [DisplayName("Estado")]
        public int idEstado { get; set; }
    }
}
