using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.Abstracciones.InterfacesAD.Remuneraciones.ObtenerRemuneracionPorId
{
    public interface IObtenerRemuneracionPorIdAD
    {
        RemuneracionDto ObtenerPorId(int idRemuneracion);
    }
}
