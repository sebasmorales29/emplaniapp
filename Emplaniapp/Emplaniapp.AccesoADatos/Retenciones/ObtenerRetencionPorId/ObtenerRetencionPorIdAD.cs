using System.Linq;
using Emplaniapp.Abstracciones.InterfacesAD.Retenciones;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.Abstracciones.ModelosAD;

namespace Emplaniapp.AccesoADatos.Retenciones
{
    public class ObtenerRetencionPorIdAD : IObtenerRetencionPorIdAD
    {
        private readonly Contexto _ctx = new Contexto();
        public RetencionDto Obtener(int idRetencion)
        {
            var q = from r in _ctx.Retenciones
                    join tr in _ctx.TipoReten on r.idTipoRetencion equals tr.Id
                    join es in _ctx.Estado on r.idEstado equals es.idEstado
                    where r.idRetencion == idRetencion
                    select new RetencionDto
                    {
                        idRetencion = r.idRetencion,
                        idEmpleado = r.idEmpleado,
                        idTipoRetencio = r.idTipoRetencion,
                        nombreTipoRetencio = tr.nombreTipoRetencion,
                        rebajo = r.rebajo ?? 0m,
                        fechaRetencio = r.fechaRetencion,
                        idEstado = r.idEstado,
                        nombreEstado = es.nombreEstado
                    };
            return q.FirstOrDefault();
        }
    }
}