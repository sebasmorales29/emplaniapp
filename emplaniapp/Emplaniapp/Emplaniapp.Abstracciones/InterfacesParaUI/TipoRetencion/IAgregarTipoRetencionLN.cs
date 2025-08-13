using System.Threading.Tasks;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.Abstracciones.InterfacesParaUI.TipoRetencion
{
    public interface IAgregarTipoRetencionLN
    {
        Task<int> Guardar(TipoRetencionDto dto);
    }
}