using System;
using Emplaniapp.Abstracciones.InterfacesAD.General.OtenerTotalEmpleados;
using Emplaniapp.Abstracciones.InterfacesParaUI.General.ObtenerTotalEmpleados;
using Emplaniapp.AccesoADatos.General.ObtenerTotalEmpleados;

namespace Emplaniapp.LogicaDeNegocio.General.ObtenerTotalEmpleados
{
    public class obtenerTotalEmpleadosLN : IObtenerTotalEmpleadosLN
    {
        private IObtenerTotalEmpleadosAD _obtenerTotalEmpleadosAD;

        public obtenerTotalEmpleadosLN()
        {
            _obtenerTotalEmpleadosAD = new obtenerTotalEmpleadosAD();
        }

        public int ObtenerTotalEmpleados(string filtro, int? idCargo, int? idEstado, bool soloActivos = false)
        {
            return _obtenerTotalEmpleadosAD.ObtenerTotalEmpleados(filtro, idCargo, idEstado, soloActivos);
        }
    }
}
