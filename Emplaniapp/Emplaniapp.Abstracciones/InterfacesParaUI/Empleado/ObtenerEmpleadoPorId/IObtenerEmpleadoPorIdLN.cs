using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.ObtenerEmpleadoPorId
{
    public interface IObtenerEmpleadoPorIdLN
    {
        EmpleadoDto ObtenerEmpleadoPorId(int idEmpleado);
    }
}
