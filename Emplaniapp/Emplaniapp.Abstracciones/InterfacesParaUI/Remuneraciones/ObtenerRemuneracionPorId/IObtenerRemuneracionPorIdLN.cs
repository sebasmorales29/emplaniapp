using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones.ObtenerRemuneracionPorId
{
    public interface IObtenerRemuneracionPorIdLN
    {
        RemuneracionDto ObtenerPorId(int idRemuneracion);
    }
}
