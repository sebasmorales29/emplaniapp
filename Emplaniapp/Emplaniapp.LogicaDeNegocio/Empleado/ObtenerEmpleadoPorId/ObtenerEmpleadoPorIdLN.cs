using Emplaniapp.Abstracciones.InterfacesAD.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Empleado.ObtenerEmpleadoPorId;

namespace Emplaniapp.LogicaDeNegocio.Empleado.ObtenerEmpleadoPorId
{
    public class ObtenerEmpleadoPorIdLN : IObtenerEmpleadoPorIdLN
    {
        private IObtenerEmpleadoPorIdAD _obtenerEmpleadoPorIdAD;

        public ObtenerEmpleadoPorIdLN( )
        {
            _obtenerEmpleadoPorIdAD = new ObtenerEmpleadoPorIdAD();
        }

        public EmpleadoDto ObtenerEmpleadoPorId(int idEmpleado)
        {
            return _obtenerEmpleadoPorIdAD.ObtenerEmpleadoPorId(idEmpleado);
        }
    }
}
