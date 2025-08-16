using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.Abstracciones.InterfacesAD.Historial
{
    public interface IConsultarHistorial
    {
        List<HistorialEmpleadoDto> ObtenerHistorialEmpleado(int idEmpleado, int? idTipoEvento = null, DateTime? fechaInicio = null, DateTime? fechaFin = null, string categoriaEvento = null, int top = 100);
        
        List<HistorialEmpleadoDto> ObtenerHistorialPorCategoria(int idEmpleado, string categoriaEvento, int top = 100);
        
        List<HistorialEmpleadoDto> ObtenerHistorialPorTipoEvento(int idEmpleado, int idTipoEvento, int top = 100);
        
        List<HistorialEmpleadoDto> ObtenerHistorialPorFecha(int idEmpleado, DateTime fechaInicio, DateTime fechaFin, int top = 100);
        
        int ObtenerTotalEventos(int idEmpleado);
        
        List<string> ObtenerCategoriasDisponibles();
    }
}
