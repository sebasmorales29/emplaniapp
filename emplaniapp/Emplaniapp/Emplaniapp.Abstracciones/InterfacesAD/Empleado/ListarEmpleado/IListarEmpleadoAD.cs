using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.Abstracciones.InterfacesAD.Empleado.ListarEmpleado
{
    public interface IListarEmpleadoAD
    {
        List<EmpleadoDto> ObtenerEmpleados();
        List<EmpleadoDto> ObtenerEmpleados(string usuarioActualId = null);
    }
}
