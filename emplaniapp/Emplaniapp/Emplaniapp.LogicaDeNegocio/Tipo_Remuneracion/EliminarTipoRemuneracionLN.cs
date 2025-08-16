using Emplaniapp.Abstracciones.InterfacesAD.Tipo_Remuneracion;
using Emplaniapp.Abstracciones.InterfacesParaUI.Tipo_Remuneracion;
using Emplaniapp.AccesoADatos.Tipo_Remuneracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.LogicaDeNegocio.Tipo_Remuneracion
{
    public class EliminarTipoRemuneracionLN : IEliminarTipoRemuneracionLN
    {
        IEliminarTipoRemuneracionAD _eliminarTR;

        public EliminarTipoRemuneracionLN()
        {
            _eliminarTR = new EliminarTipoRemuneracionAD();
        }

        public int Eliminar(int id)
        {
            int seEliminoTR = _eliminarTR.Eliminar(id);
            return seEliminoTR;
        }


    }
}
