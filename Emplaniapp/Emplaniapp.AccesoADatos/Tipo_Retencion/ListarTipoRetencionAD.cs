using Emplaniapp.Abstracciones.InterfacesAD.Tipo_Retencion;
using Emplaniapp.Abstracciones.ModelosAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.AccesoADatos.Tipo_Retencion
{
    public class ListarTipoRetencionAD : IListarTipoRetencionAD
    {
        Contexto contexto;

        public ListarTipoRetencionAD()
        {
            contexto = new Contexto();
        }

        public List<TipoRetencion> Listar()
        {
            List<TipoRetencion> TRet = contexto.TipoReten.ToList();
            return TRet;
        }


    }
}
