using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.InterfacesParaUI.Hoja_Resumen.AprobarEmpleadosHojaResumen
{
    public interface IAprobarEmpleadosHojaResumenLN
    {
        bool AprobarPagoQuincenal(int idPagoQuincenal, string idUsuario);
    }
}
