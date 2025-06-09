using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Hoja_Resumen.ListarHojaResumen;
using Emplaniapp.Abstracciones.InterfacesParaUI.Hoja_Resumen.ListarHojaResumen;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Hoja_Resumen;

namespace Emplaniapp.LogicaDeNegocio.Hoja_Resumen.ListarHojaResumen
{
    public class listarHojaResumenLN: IlistarHojaResumenLN
    {
        private IListarHojaResumenAD _listarHojaResumenAD;

        public listarHojaResumenLN()
        {
            _listarHojaResumenAD =  new listarHojaResumenAD();
        }
        public List<HojaResumenDto> ObtenerHojasResumen()
        {
            return _listarHojaResumenAD.ObtenerHojasResumen();
        }


        public List<HojaResumenDto> ObtenerFiltrado(string filtro, int? idCargo, int? idEstado)
        {
            return _listarHojaResumenAD.ObtenerFiltrado(filtro, idCargo, idEstado);
        }

        public List<CargoDto> ObtenerCargos()
        {
            return _listarHojaResumenAD.ObtenerCargos();
        }

        public int ObtenerTotalEmpleados(string filtro, int? idCargo, int? idEstado)
        {
            return _listarHojaResumenAD.ObtenerTotalEmpleados(filtro, idCargo, idEstado);
        }

        public List<EstadoDto> ObtenerEstados()
        {
            return _listarHojaResumenAD.ObtenerEstados();
        }

        public bool CambiarEstadoEmpleado(int idEmpleado, int idEstado)
        {
            return _listarHojaResumenAD.CambiarEstadoEmpleado(idEmpleado, idEstado);
        }

        public EmpleadoDto ObtenerEmpleadoPorId(int idEmpleado)
        {
            return _listarHojaResumenAD.ObtenerEmpleadoPorId(idEmpleado);
        }
    }
}
