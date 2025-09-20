using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.Abstracciones.InterfacesParaUI.PeriodoPago.CrearPeriodoPago
{
    public interface ICrearPeriodoPagoLN
    {
        List<PeriodoPagoDto> GenerarPeriodoPago(DateTime? fechaProceso = null);
    }
}
