using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.Abstracciones.InterfacesAD.Historial
{
    public interface IRegistrarEventoHistorial
    {
        bool RegistrarEvento(int idEmpleado, string nombreEvento, string descripcionEvento, string detallesEvento = null, string valorAnterior = null, string valorNuevo = null, string idUsuarioModificacion = null, string ipModificacion = null);
        
        bool RegistrarEventoPersonalizado(int idEmpleado, string nombreEvento, string descripcionEvento, string categoriaEvento = "Sistema", string iconoEvento = "info-circle", string colorEvento = "secondary", string detallesEvento = null, string valorAnterior = null, string valorNuevo = null, string idUsuarioModificacion = null, string ipModificacion = null);
        
        bool RegistrarCambioEmpleado(int idEmpleado, string nombreEvento, string descripcionEvento, string valorAnterior, string valorNuevo, string idUsuarioModificacion = null, string ipModificacion = null);
        
        bool RegistrarCreacionEmpleado(int idEmpleado, string idUsuarioModificacion = null, string ipModificacion = null);
        
        bool RegistrarLiquidacion(int idEmpleado, string motivo, decimal costoTotal, string idUsuarioModificacion = null, string ipModificacion = null);
        
        bool RegistrarRemuneracion(int idEmpleado, decimal monto, string tipoRemuneracion, string idUsuarioModificacion = null, string ipModificacion = null);
        
        bool RegistrarRetencion(int idEmpleado, decimal monto, string tipoRetencion, string idUsuarioModificacion = null, string ipModificacion = null);
    }
}
