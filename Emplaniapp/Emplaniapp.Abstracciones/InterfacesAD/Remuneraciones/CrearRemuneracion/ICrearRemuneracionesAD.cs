using System;
using System.Collections.Generic;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.Abstracciones.InterfacesAD.Remuneraciones.CrearRemuneracion
{
    public interface ICrearRemuneracionesAD
    {
        List<RemuneracionDto> GenerarRemuneracionesQuincenales(DateTime? fechaProceso = null);
        void AgregarRemuneracionManual(RemuneracionDto remuneracionDto, int idEmpleado);
    }
}
