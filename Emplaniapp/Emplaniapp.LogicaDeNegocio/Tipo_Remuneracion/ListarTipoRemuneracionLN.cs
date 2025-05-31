using Emplaniapp.Abstracciones.InterfacesParaUI.Tipo_Remuneracion;
using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.LogicaDeNegocio.Tipo_Remuneracion
{
    public class ListarTipoRemuneracionLN : IListarTipoRemuneracionLN
    {
        Contexto contexto;

        public ListarTipoRemuneracionLN() 
        {
            contexto = new Contexto();
        }


        // Base de Datos
        public List<TipoRemuneracion> ListarTipoRemuneracion()
        {
            List<TipoRemuneracion> TRem = contexto.TipoRemu.ToList();
            return TRem;
        }


        // Para UI
        public List<TipoRemuneracionDto> Listar()
        {
            List<TipoRemuneracionDto> TRemuneraciones =
                (from remu in contexto.TipoRemu
                 select new TipoRemuneracionDto
                 {
                     Id = remu.Id,
                     nombreTipoRemuneracion = remu.nombreTipoRemuneracion,
                     porcentajeRemuneracion = remu.porcentajeRemuneracion,
                     idEstado = remu.idEstado
                 }
                 ).ToList();
            return TRemuneraciones;
        }



    }
}
