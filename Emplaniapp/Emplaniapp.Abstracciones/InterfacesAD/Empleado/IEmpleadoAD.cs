using Emplaniapp.Abstracciones.ModelosAD;
using System.Collections.Generic;

namespace Emplaniapp.Abstracciones.InterfacesAD.Empleado
{
    public interface IEmpleadoAD
    {
        List<ModelosAD.Empleado> ListarTodos();
        ModelosAD.Empleado ObtenerPorId(int id);
        bool Insertar(ModelosAD.Empleado empleado);
        bool Actualizar(ModelosAD.Empleado empleado);
        bool Eliminar(int id);
        bool ActivarDesactivar(int id, int nuevoEstado);
    }
} 