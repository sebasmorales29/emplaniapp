using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.InterfacesAD.Hoja_Resumen.AprobarEmpleadosHojaResumen
{
    public interface IAprobarEmpleadosHojaResumenAD
    {
        bool AprobarPagoQuincenal(int idPagoQuincenal, string idUsuario);
    }
}
