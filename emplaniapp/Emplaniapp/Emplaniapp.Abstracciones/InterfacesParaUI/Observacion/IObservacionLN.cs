using Emplaniapp.Abstracciones.ModelosParaUI;
using System.Collections.Generic;

namespace Emplaniapp.Abstracciones.InterfacesParaUI
{
    public interface IObservacionLN
    {
        List<ObservacionDto> ObtenerObservacionesPorEmpleado(int idEmpleado);
        ObservacionDto ObtenerObservacionPorId(int idObservacion);
        bool GuardarObservacion(ObservacionDto observacionDto);
        bool ActualizarObservacion(ObservacionDto observacionDto);
        bool EliminarObservacion(int idObservacion);
    }
} 