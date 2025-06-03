using Emplaniapp.Abstracciones.InterfacesAD.Tipo_Retencion;
using Emplaniapp.Abstracciones.InterfacesParaUI.Tipo_Retenciones;
using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Tipo_Retencion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.LogicaDeNegocio.Tipo_Retencion
{
    public class ObtenerIdTipoRetencionLN : IObtenerIdTipoRetencionLN
    {

        IObtenerIdTipoRetencionAD _obtenerTR;

        public ObtenerIdTipoRetencionLN()
        {
            _obtenerTR = new ObtenerIdTipoRetencionAD();
        }

        // Método público ------------------------------------------------------
        public TipoRetencionDto Obtener(int id)
        {
            TipoRetencion trEnBD = _obtenerTR.Obtener(id);       // Estructura en BD
            TipoRetencionDto tipoRetPresentar = ObtenerRepuesto(trEnBD); // Estructura en Presentacion
            return tipoRetPresentar;
        }


        // Obtener un objeto de la lista --------------------------------------------------------------
        private TipoRetencionDto ObtenerRepuesto(TipoRetencion tipoReten) // Esta acción separada mantiene el código limpio
        {
            return new TipoRetencionDto
            {
                Id = tipoReten.Id,
                nombreTipoRetencion = tipoReten.nombreTipoRetencion,
                porcentajeRetencion = tipoReten.porcentajeRetencion,
                idEstado = tipoReten.idEstado
            };
        }

    }
}
