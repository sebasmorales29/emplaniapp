using Emplaniapp.Abstracciones.Entidades;
using System.Collections.Generic;

namespace Emplaniapp.Abstracciones.InterfacesAD
{
    public interface IObservacionAD
    {
        List<Observacion> ObtenerObservacionesPorEmpleado(int idEmpleado);
        Observacion ObtenerObservacionPorId(int idObservacion);
        bool GuardarObservacion(Observacion observacion);
        bool ActualizarObservacion(Abstracciones.ModelosParaUI.ObservacionDto observacionDto);
        bool EliminarObservacion(int idObservacion);
    }
} 