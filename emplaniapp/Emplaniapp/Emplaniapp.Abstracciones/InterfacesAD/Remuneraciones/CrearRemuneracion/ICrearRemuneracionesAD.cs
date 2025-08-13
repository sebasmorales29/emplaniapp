using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.Abstracciones.InterfacesAD.Remuneraciones.CrearRemuneracion
{
    public interface ICrearRemuneracionesAD
    {
        List<RemuneracionDto> GenerarRemuneracionesQuincenales(DateTime? fechaProceso = null);
        Task<int> AgregarRemuneracionManual(RemuneracionDto remuneracionDto);
    }
}
