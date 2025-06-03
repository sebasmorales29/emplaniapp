using Emplaniapp.Abstracciones.InterfacesAD.Tipo_Retencion;
using Emplaniapp.Abstracciones.ModelosAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.AccesoADatos.Tipo_Retencion
{
    public class ObtenerIdTipoRetencionAD : IObtenerIdTipoRetencionAD
    {
        Contexto contexto;

        public ObtenerIdTipoRetencionAD()
        {
            contexto = new Contexto();
        }


        public TipoRetencion Obtener(int id)
        {
            TipoRetencion tipoRet = contexto.TipoReten.
                         Where(tret => tret.Id == id).FirstOrDefault();
            return tipoRet;
        }
         

    }
}
