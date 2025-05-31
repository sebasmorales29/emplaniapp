using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.Abstracciones.ModelosParaUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.InterfacesParaUI.Tipo_Retenciones
{
    public interface IListarTipoRetencionLN
    {
        List<TipoRetencion> ListarTipoRetencion();
        List<TipoRetencionDto> Listar();
    }
}
