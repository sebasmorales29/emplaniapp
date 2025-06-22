using Emplaniapp.Abstracciones.InterfacesAD.Retenciones;
using Emplaniapp.Abstracciones.ModelosParaUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.AccesoADatos.Retenciones
{
    public class ListarRetencionesAD : IListarRetencionesAD
    {
        Contexto contexto;

        public ListarRetencionesAD()
        {
            contexto = new Contexto();
        }

        public List<RetencionDto> Listar(int id)
        {
            List<RetencionDto> ListaReten =
                (from reten in contexto.Retenciones
                 join t_reten in contexto.TipoReten on reten.idTipoRetencio equals t_reten.Id
                 join estado in contexto.Estado on reten.idEstado equals estado.idEstado
                 select new RetencionDto
                 {
                     idRetencion = reten.idRetencion,
                     idEmpleado = reten.idEmpleado,
                     idTipoRetencio = reten.idTipoRetencio,
                     nombreTipoRetencio = t_reten.nombreTipoRetencion,
                     rebajo = reten.rebajo,
                     fechaRetencio = reten.fechaRetencio,
                     idEstado = reten.idEstado,
                     nombreEstado = estado.nombreEstado
                 })
                .Where(retencion => retencion.idEmpleado == id)
                .ToList();
            return ListaReten;
        }


    }
}
