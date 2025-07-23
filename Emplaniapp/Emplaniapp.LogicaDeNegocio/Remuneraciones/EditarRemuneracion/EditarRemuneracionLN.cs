using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.Abstracciones.InterfacesAD.Remuneraciones.EditarRemuneracion;
using Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones.EditarRemuneracion;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.AccesoADatos.Remuneraciones.EditarRemuneracion;

namespace Emplaniapp.LogicaDeNegocio.Remuneraciones.EditarRemuneracion
{
    public class EditarRemuneracionLN : IEditarRemuneracionLN
    {
        private IEditarRemuneracionAD _editarRemuneracionAD;
        private IObtenerEmpleadoPorIdAD _obtenerEmpleadoPorIdAD;

        public EditarRemuneracionLN()
        {
            _editarRemuneracionAD = new EditarRemuneracionAD();
            _obtenerEmpleadoPorIdAD = new ObtenerEmpleadoPorIdAD();
        }

        public int Actualizar(RemuneracionDto laRemuneracion)
        {
            int cantidadDeResultados = _editarRemuneracionAD.Actualizar(laRemuneracion);
            return cantidadDeResultados;
        }
    }
}
