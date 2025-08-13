using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Tipo_Retencion;
using Emplaniapp.Abstracciones.ModelosAD;

namespace Emplaniapp.AccesoADatos.Tipo_Retencion
{
    public class AgregarTipoRetencionAD : IAgregarTipoRetencionAD
    {
        private readonly Contexto _ctx = new Contexto();
        public async Task<int> AgregarAsync(TipoRetencion entidad)
        {
            entidad.idEstado = 1;
            _ctx.TipoReten.Add(entidad);
            return await _ctx.SaveChangesAsync();
        }
    }
}