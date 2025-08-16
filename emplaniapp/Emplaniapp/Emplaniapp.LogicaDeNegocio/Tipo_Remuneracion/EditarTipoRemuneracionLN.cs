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
    public class EditarTipoRemuneracionLN : IEditarTipoRemuneracionLN
    {

        IEditarTipoRemuneracionAD _editarTR;

        public EditarTipoRemuneracionLN()
        {
            _editarTR = new EditarTipoRemuneracionAD();
        }


        // Para actualizar el valor -------------------------------------------------------------

        public int Editar(TipoRemuneracionDto tipoRemu)
        {
            int seActualizoTR = _editarTR.Editar(CambioABaseDatos(tipoRemu));
            return seActualizoTR;
        }

        private TipoRemuneracion CambioABaseDatos(TipoRemuneracionDto tipoRemu) // Esta acción separada mantiene el código limpio
        {
            return new TipoRemuneracion
            {
                Id = tipoRemu.Id,
                nombreTipoRemuneracion = tipoRemu.nombreTipoRemuneracion,
                porcentajeRemuneracion = tipoRemu.porcentajeRemuneracion,
                idEstado = tipoRemu.idEstado
            };
        }


    }
}
