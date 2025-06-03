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
    public class EditarTipoRetencionLN : IEditarTipoRetencionLN
    {
        IEditarTipoRetencionAD _editarTR;

        public EditarTipoRetencionLN()
        {
            _editarTR = new EditarTipoRetencionAD();
        }

       
        public int Editar(TipoRetencionDto tipoReten)
        {
            int seActualizoTR = _editarTR.Editar(CambioABaseDatos(tipoReten));
            return seActualizoTR;
        }

        private TipoRetencion CambioABaseDatos(TipoRetencionDto tipoReten) // Esta acción separada mantiene el código limpio
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
