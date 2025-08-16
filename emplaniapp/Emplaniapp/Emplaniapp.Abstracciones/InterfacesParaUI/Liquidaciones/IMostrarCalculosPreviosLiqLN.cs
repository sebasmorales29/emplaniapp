using Emplaniapp.Abstracciones.ModelosParaUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.InterfacesParaUI.Liquidaciones
{
    public interface IMostrarCalculosPreviosLiqLN
    {
        LiquidacionDto MostrarLiquidacionParcial(EmpleadoDto emp);
        LiquidacionDto MostrarLiquidacionTotal(EmpleadoDto emp, DateTime fechaliq, string motivo);
    }
}
