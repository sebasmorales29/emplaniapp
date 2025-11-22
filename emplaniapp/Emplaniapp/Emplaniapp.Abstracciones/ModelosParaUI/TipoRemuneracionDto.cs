using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosParaUI
{
    public class TipoRemuneracionDto
    {
        public int Id { get; set; }

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El nombre de la remuneración es obligatorio.")]
        public string nombreTipoRemuneracion { get; set; }


        [DisplayName("Porcentaje %")]
        [Required(ErrorMessage = "El porcentaje de remuneración es obligatorio.")]
        [RegularExpression("^([0-9])$", ErrorMessage ="Valor debe ser un número")]
        public double porcentajeRemuneracion { get; set; }


        [DisplayName("Estado")]
        public int idEstado { get; set; }
    }
}
