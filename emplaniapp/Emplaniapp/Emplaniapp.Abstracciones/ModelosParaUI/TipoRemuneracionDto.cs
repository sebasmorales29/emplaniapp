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
        public string nombreTipoRemuneracion { get; set; }

        [DisplayName("Porcentaje %")]
        public double porcentajeRemuneracion { get; set; }

        [DisplayName("Estado")]
        public int idEstado { get; set; }
    }
}
