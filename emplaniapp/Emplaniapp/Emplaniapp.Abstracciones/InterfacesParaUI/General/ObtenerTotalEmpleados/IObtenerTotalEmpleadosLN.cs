using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.InterfacesParaUI.General.ObtenerTotalEmpleados
{
    public interface IObtenerTotalEmpleadosLN
    {
        int ObtenerTotalEmpleados(string filtro, int? idCargo, int? idEstado, bool soloActivos = false);
    }
}
