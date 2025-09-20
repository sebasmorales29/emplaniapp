using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Hoja_Resumen.AprobarEmpleadosHojaResumen;
using Emplaniapp.Abstracciones.InterfacesParaUI.Hoja_Resumen.AprobarEmpleadosHojaResumen;
using Emplaniapp.AccesoADatos.Hoja_Resumen.AprobarEmpleadosHojaResumen;

namespace Emplaniapp.LogicaDeNegocio.Hoja_Resumen.AprobarEmpleadosHojaResumen
{
    public class AprobarEmpleadosHojaResumenLN : IAprobarEmpleadosHojaResumenLN
    {
        private IAprobarEmpleadosHojaResumenAD _aprobarEmpleadosHojaResumenAD;

        public AprobarEmpleadosHojaResumenLN()
        {
            _aprobarEmpleadosHojaResumenAD = new AprobarEmpleadosHojaResumenAD();
        }

        public bool AprobarPagoQuincenal(int idPagoQuincenal, string idUsuario)
        {
            if (idPagoQuincenal <= 0 || string.IsNullOrWhiteSpace(idUsuario))
                return false;

            return _aprobarEmpleadosHojaResumenAD.AprobarPagoQuincenal(idPagoQuincenal, idUsuario);
        }
    }
}