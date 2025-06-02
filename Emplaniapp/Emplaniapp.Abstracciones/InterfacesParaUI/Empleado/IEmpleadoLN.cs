using Emplaniapp.Abstracciones.ModelosParaUI;
using System.Collections.Generic;

namespace Emplaniapp.Abstracciones.InterfacesParaUI.Empleado
{
    public interface IEmpleadoLN
    {
        List<EmpleadoDto> ListarTodos();
        EmpleadoDto ObtenerPorId(int id);
        bool Insertar(EmpleadoDto empleado);
        bool Actualizar(EmpleadoDto empleado);
        bool Eliminar(int id);
        bool ActivarDesactivar(int id, int nuevoEstado);
        
        // Métodos específicos para obtener datos de catálogos
        List<CargoDto> ObtenerCargos();
        List<MonedaDto> ObtenerTiposMoneda();
        List<BancoDto> ObtenerBancos();
        List<EstadoDto> ObtenerEstados();
    }
} 