using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.Abstracciones.InterfacesAD.Hoja_Resumen.ListarHojaResumen
{
    public interface IListarHojaResumenAD
    {
        List<HojaResumenDto> ObtenerHojasResumen();
    }
}
