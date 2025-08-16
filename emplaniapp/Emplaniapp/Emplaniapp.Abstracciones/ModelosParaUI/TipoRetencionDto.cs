using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosParaUI
{
    public class TipoRetencionDto
    {
        public int Id { get; set; }

        [DisplayName("Nombre")]
        public string nombreTipoRetencion { get; set; }

        [DisplayName("Porcentaje %")]
        public double porcentajeRetencion { get; set; }
        public int idEstado { get; set; }
    }
}
