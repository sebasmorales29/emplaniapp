using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.General.FiltrarEmpleados;
using Emplaniapp.Abstracciones.InterfacesParaUI.General.FiltrarEmpleados;
using Emplaniapp.AccesoADatos.General.Filtrar;

namespace Emplaniapp.LogicaDeNegocio.General.FiltrarEmpleados
{
    public class filtrarEmpleadosLN : IFiltrarEmpleadosLN
    {
        private IFiltrarEmpleadosAD _filtrarEmpleadosAD;
        public filtrarEmpleadosLN()
        {
            _filtrarEmpleadosAD = new filtrarEmpleadosAD();
        }

        public List<T> ObtenerFiltrado<T>(string filtro, int? idCargo, int? idEstado) where T : class
        {
            return _filtrarEmpleadosAD.ObtenerFiltrado<T>(filtro, idCargo, idEstado);
        }

        public List<T> ObtenerFiltrado<T>(string filtro, int? idCargo, int? idEstado, string usuarioActualId = null) where T : class
        {
            return _filtrarEmpleadosAD.ObtenerFiltrado<T>(filtro, idCargo, idEstado, usuarioActualId);
        }
    }
}
