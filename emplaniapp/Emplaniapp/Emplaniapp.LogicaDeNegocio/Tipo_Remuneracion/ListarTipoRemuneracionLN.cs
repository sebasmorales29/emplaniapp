using Emplaniapp.Abstracciones.InterfacesAD.Tipo_Remuneracion;
using Emplaniapp.Abstracciones.InterfacesParaUI.Tipo_Remuneracion;
using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos;
using Emplaniapp.AccesoADatos.Tipo_Remuneracion;
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
        private IListarTipoRemuneracionAD _listarTipoRemuneracionAD;

        public ListarTipoRemuneracionLN() 
        {
            contexto = new Contexto();
            _listarTipoRemuneracionAD = new ListarTipoRemuneracionAD();
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

        public List<TipoRemuneracionDto> ObtenerTipoRemuneracion()
        {
            return _listarTipoRemuneracionAD.ObtenerTipoRemuneracion();
        }



    }
}
