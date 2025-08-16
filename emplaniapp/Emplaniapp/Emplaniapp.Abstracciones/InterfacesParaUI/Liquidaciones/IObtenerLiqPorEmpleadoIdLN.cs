using Emplaniapp.Abstracciones.ModelosParaUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.InterfacesParaUI.Liquidaciones
{
    public interface IObtenerLiqPorEmpleadoIdLN
    {
        LiquidacionDto ObtenerPorEmpleadoID(int idEmp);
        LiquidacionDto Activa(int idEmp);
    }
}
