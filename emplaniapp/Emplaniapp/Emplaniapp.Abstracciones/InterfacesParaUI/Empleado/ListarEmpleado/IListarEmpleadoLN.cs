using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.ListarEmpleado
{
    public interface IListarEmpleadoLN
    {
        List<EmpleadoDto> ObtenerEmpleados();
        List<EmpleadoDto> ObtenerEmpleados(string usuarioActualId = null);
    }
}
