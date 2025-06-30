using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones.CrearRemuneraciones
{
    public interface ICrearRemuneracionesLN
    {
        List<RemuneracionDto> GenerarRemuneracionesQuincenales(DateTime? fechaProceso = null);
        Task<int> AgregarRemuneracionManual(RemuneracionDto remuneracionDto);
    }
}