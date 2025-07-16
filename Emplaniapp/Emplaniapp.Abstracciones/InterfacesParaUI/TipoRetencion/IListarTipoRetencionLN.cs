using System.Collections.Generic;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.Abstracciones.InterfacesParaUI.TipoRetencion
{
    public interface IListarTipoRetencionLN
    {
        List<TipoRetencionDto> Listar();
    }
}