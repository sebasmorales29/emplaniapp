using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Remuneraciones.EliminarRemuneracion;
using Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones.EliminarRemuneracion;
using Emplaniapp.AccesoADatos.Remuneraciones.EliminarRemuneracion;

namespace Emplaniapp.LogicaDeNegocio.Remuneraciones.EliminarRemuneracion
{
    public class EliminarRemuneracionLN : IEliminarRemuneracionLN
    {
        private readonly IEliminarRemuneracionAD _eliminarRemuneracionAD;
        public EliminarRemuneracionLN()
        {
            _eliminarRemuneracionAD = new EliminarRemuneracionAD();
        }
        public bool EliminarRemuneracion(int idRemuneracion)
        {
            return _eliminarRemuneracionAD.EliminarRemuneracion(idRemuneracion);
        }
    }
}
