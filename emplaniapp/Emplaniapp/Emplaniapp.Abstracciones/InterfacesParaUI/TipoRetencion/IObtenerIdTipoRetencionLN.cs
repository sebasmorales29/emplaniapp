using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.Abstracciones.InterfacesParaUI.TipoRetencion
{
    public interface IObtenerIdTipoRetencionLN
    {
        TipoRetencionDto Obtener(int idTipoRetencion);
    }
}