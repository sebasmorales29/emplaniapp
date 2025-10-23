using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.InterfacesParaUI.General.FiltrarEmpleados
{
    public interface IFiltrarEmpleadosLN
    {
        List<T> ObtenerFiltrado<T>(string filtro, int? idCargo, int? idEstado) where T : class;
        List<T> ObtenerFiltrado<T>(string filtro, int? idCargo, int? idEstado, string usuarioActualId = null) where T : class;
    }
}
