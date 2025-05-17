using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosParaUI
{
    public class DireccionDto
    {
        public int idDireccion { get; set; }
        public int idProvincia { get; set; }
        public string nombreProvincia { get; set; }
        public int idCanton { get; set; }
        public string nombreCanton { get; set; }
        public int idDistrito { get; set; }
        public string nombreDistrito { get; set; }
        public int idCalle { get; set; }
        public string nombreCalle { get; set; }
    }
}
