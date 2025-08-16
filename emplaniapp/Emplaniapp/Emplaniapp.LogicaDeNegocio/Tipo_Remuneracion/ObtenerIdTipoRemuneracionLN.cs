using Emplaniapp.Abstracciones.InterfacesAD.Tipo_Remuneracion;
using Emplaniapp.Abstracciones.InterfacesParaUI.Tipo_Remuneracion;
using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Tipo_Remuneracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.LogicaDeNegocio.Tipo_Remuneracion
{
    public class ObtenerIdTipoRemuneracionLN : IObtenerIdTipoRemuneracionLN
    {
        IObtenerIdTipoRemuneracionAD _obtenerTR;

        public ObtenerIdTipoRemuneracionLN()
        {
            _obtenerTR = new ObtenerIdTipoRemuneracionAD();
        }



        // Método público ------------------------------------------------------
        public TipoRemuneracionDto Obtener(int id)
        {
            TipoRemuneracion trEnBD = _obtenerTR.Obtener(id);       // Estructura en BD
            TipoRemuneracionDto tipoRemuPresentar = ObtenerRepuesto(trEnBD); // Estructura en Presentacion
            return tipoRemuPresentar;
        }


        // Obtener un objeto de la lista --------------------------------------------------------------
        private TipoRemuneracionDto ObtenerRepuesto(TipoRemuneracion tipoRemu) // Esta acción separada mantiene el código limpio
        {
            return new TipoRemuneracionDto
            {
               Id = tipoRemu.Id,
               nombreTipoRemuneracion = tipoRemu.nombreTipoRemuneracion,
               porcentajeRemuneracion = tipoRemu.porcentajeRemuneracion,
               idEstado = tipoRemu.idEstado
            };
        }


    }
}
