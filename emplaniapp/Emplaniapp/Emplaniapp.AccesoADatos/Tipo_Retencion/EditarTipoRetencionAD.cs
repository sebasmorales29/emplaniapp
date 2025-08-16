using Emplaniapp.Abstracciones.InterfacesAD.Tipo_Retencion;
using Emplaniapp.Abstracciones.ModelosAD;

namespace Emplaniapp.AccesoADatos.Tipo_Retencion
{
    public class EditarTipoRetencionAD : IEditarTipoRetencionAD
    {
        private readonly Contexto _ctx = new Contexto();
        public int Editar(TipoRetencion entidad)
        {
            var e = _ctx.TipoReten.Find(entidad.Id);
            if (e == null) return 0;
            e.nombreTipoRetencion = entidad.nombreTipoRetencion;
            e.porcentajeRetencion = entidad.porcentajeRetencion;
            e.idEstado = entidad.idEstado;
            return _ctx.SaveChanges();
        }
    }
}