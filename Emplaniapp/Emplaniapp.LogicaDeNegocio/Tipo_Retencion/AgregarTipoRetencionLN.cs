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
    public class AgregarTipoRetencionLN : IAgregarTipoRetencionLN
    {
        IAgregarTipoRetencionAD _agregarTR;

        public AgregarTipoRetencionLN()
        {
            _agregarTR = new AgregarTipoRetencionAD();
        }

        
        public async Task<int> Guardar(TipoRetencionDto tipoReten) // Para guardar un archivo
        {
            int seGuardoTR= await _agregarTR.Agregar(ObtenerTR(tipoReten));
            return seGuardoTR;
        }

   
        private TipoRetencion ObtenerTR(TipoRetencionDto tipoReten) // Esta acción separada mantiene el código limpio
        {
            return new TipoRetencion
            {
                Id = tipoReten.Id,
                nombreTipoRetencion = tipoReten.nombreTipoRetencion,
                porcentajeRetencion = tipoReten.porcentajeRetencion,
                idEstado = tipoReten.idEstado
            };
        }
         
         




    }
}
