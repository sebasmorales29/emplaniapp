using Emplaniapp.Abstracciones.ModelosParaUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.InterfacesParaUI.Liquidaciones
{
    public interface IGenerarCalculoLiqLN
    {
        LiquidacionDto PrimerCalculo(EmpleadoDto emp, DateTime fechaliq, string motivo, string observs);
        LiquidacionDto ModificarCalculo(LiquidacionDto liq);
    }
}
