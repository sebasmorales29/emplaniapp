using System.Threading.Tasks;
using Emplaniapp.Abstracciones.ModelosAD;

namespace Emplaniapp.Abstracciones.InterfacesAD.Tipo_Retencion
{
    public interface IAgregarTipoRetencionAD
    {
        Task<int> AgregarAsync(TipoRetencion entidad);
    }
}