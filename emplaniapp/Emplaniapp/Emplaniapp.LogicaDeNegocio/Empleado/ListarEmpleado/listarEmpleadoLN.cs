using System.Collections.Generic;
using Emplaniapp.Abstracciones.InterfacesAD.Empleado.ListarEmpleado;
using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.ListarEmpleado;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Empleado.listarEmpleado;

namespace Emplaniapp.LogicaDeNegocio.Empleado.ListarEmpleado
{
    public class listarEmpleadoLN : IListarEmpleadoLN
    {
        private IListarEmpleadoAD _listarEmpleadoAD;
        public listarEmpleadoLN()
        {
            _listarEmpleadoAD = new listarEmpleadoAD();
        }

        public List<EmpleadoDto> ObtenerEmpleados()
        {
            return _listarEmpleadoAD.ObtenerEmpleados();
        }

        public List<EmpleadoDto> ObtenerEmpleados(string usuarioActualId = null)
        {
            return _listarEmpleadoAD.ObtenerEmpleados(usuarioActualId);
        }
    }
}
