using Emplaniapp.Abstracciones.ModelosAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.InterfacesAD.Tipo_Retencion
{
    public interface IObtenerIdTipoRetencionAD
    {
        TipoRetencion Obtener(int id);
    }
}
