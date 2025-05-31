using Emplaniapp.Abstracciones.InterfacesParaUI.Tipo_Retenciones;
using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.LogicaDeNegocio.Tipo_Retencion
{
    public class ListarTipoRetencionLN : IListarTipoRetencionLN 
    {
        Contexto contexto;

        public ListarTipoRetencionLN()
        {
            contexto = new Contexto();
        }

        // Base de Datos
        public List<TipoRetencion> ListarTipoRetencion()
        {
            List<TipoRetencion> TRet = contexto.TipoReten.ToList();
            return TRet;
        }


        // Para UI
        public List<TipoRetencionDto> Listar()
        {
            List<TipoRetencionDto> TRetenciones =
                (from reten in contexto.TipoReten
                 select new TipoRetencionDto
                 {
                     Id = reten.Id,
                     nombreTipoRetencion = reten.nombreTipoRetencion,
                     porcentajeRetencion = reten.porcentajeRetencion,
                     idEstado = reten.idEstado
                 }
                 ).ToList();
            return TRetenciones;
        }


    }
}
