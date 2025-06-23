using Emplaniapp.Abstracciones.InterfacesAD.Tipo_Remuneracion;
using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.Abstracciones.ModelosParaUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

        public List<TipoRemuneracionDto> ObtenerTipoRemuneracion()
        {
            return contexto.TipoRemu
                .Select(tR => new TipoRemuneracionDto
                {
                    Id = tR.Id,
                    nombreTipoRemuneracion = tR.nombreTipoRemuneracion
                }).ToList();
        }


    }
}
