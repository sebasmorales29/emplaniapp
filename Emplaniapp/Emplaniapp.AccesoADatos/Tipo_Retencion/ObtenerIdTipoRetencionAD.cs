using Emplaniapp.Abstracciones.InterfacesAD.Tipo_Retencion;
using Emplaniapp.Abstracciones.ModelosAD;

namespace Emplaniapp.AccesoADatos.Tipo_Retencion
{
    public class ObtenerIdTipoRetencionAD : IObtenerIdTipoRetencionAD
    {
        private readonly Contexto _ctx = new Contexto();
        public TipoRetencion Obtener(int idTipoRetencion) =>
            _ctx.TipoReten.Find(idTipoRetencion);
    }
}