using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.Abstracciones.InterfacesAD.Retenciones
{
    public interface IObtenerRetencionPorIdAD
    {
        RetencionDto Obtener(int idRetencion);
    }
}