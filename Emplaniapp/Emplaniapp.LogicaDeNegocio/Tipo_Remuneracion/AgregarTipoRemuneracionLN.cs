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
    public class AgregarTipoRemuneracionLN : IAgregarTipoRemuneracionLN
    {
        IAgregarTipoRemuneracionAD _agregarTR;

        public AgregarTipoRemuneracionLN() 
        {
            _agregarTR = new AgregarTipoRemuneracionAD();
        }


        public async Task<int> Guardar(TipoRemuneracionDto tipoRemu) // Para guardar un archivo
        {
            int seGuardoTR= await _agregarTR.Agregar(ObtenerTR(tipoRemu));
            return seGuardoTR;
        }

   
        private TipoRemuneracion ObtenerTR(TipoRemuneracionDto tipoRemu) // Esta acción separada mantiene el código limpio
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
