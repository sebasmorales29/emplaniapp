using Emplaniapp.Abstracciones.InterfacesAD.Tipo_Remuneracion;
using Emplaniapp.Abstracciones.ModelosAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.AccesoADatos.Tipo_Remuneracion
{
    public class ObtenerIdTipoRemuneracionAD : IObtenerIdTipoRemuneracionAD
    {
        Contexto contexto;

        public ObtenerIdTipoRemuneracionAD()
        {
            contexto = new Contexto();
        }

        public TipoRemuneracion Obtener(int id)
        {
            TipoRemuneracion tipoRemu = contexto.TipoRemu.
                         Where(tremu => tremu.Id == id).FirstOrDefault();
            return tipoRemu;
        }
    }
}
