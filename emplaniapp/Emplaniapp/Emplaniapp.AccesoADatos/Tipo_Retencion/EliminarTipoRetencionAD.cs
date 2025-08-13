using Emplaniapp.Abstracciones.InterfacesAD.Tipo_Retencion;

namespace Emplaniapp.AccesoADatos.Tipo_Retencion
{
    public class EliminarTipoRetencionAD : IEliminarTipoRetencionAD
    {
        private readonly Contexto _ctx = new Contexto();
        public int Eliminar(int idTipoRetencion)
        {
            var e = _ctx.TipoReten.Find(idTipoRetencion);
            if (e == null) return 0;
            _ctx.TipoReten.Remove(e);
            return _ctx.SaveChanges();
        }
    }
}