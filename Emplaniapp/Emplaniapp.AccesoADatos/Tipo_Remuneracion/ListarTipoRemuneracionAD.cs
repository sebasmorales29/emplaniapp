using Emplaniapp.Abstracciones.InterfacesAD.Tipo_Remuneracion;
using Emplaniapp.Abstracciones.ModelosAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.AccesoADatos.Tipo_Remuneracion
{
    public class ListarTipoRemuneracionAD : IListarTipoRemuneracionAD
    {

        Contexto contexto;

        public ListarTipoRemuneracionAD()
        {
            contexto = new Contexto();
        }


        public List<TipoRemuneracion> Listar()
        {
            List<TipoRemuneracion> TRem = contexto.TipoRemu.ToList();
            return TRem;
        }


    }
}
