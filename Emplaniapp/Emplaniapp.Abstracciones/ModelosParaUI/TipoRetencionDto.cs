using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosParaUI
{
    public class TipoRetencionDto
    {
        public int Id { get; set; }
        public string nombreTipoRetencion { get; set; }
        public double porcentajeRetencion { get; set; }
        public int idEstado { get; set; }
    }
}
