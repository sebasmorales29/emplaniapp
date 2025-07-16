using Emplaniapp.Abstracciones.InterfacesAD.Remuneraciones.ObtenerRemuneracionPorId;
using Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones.ObtenerRemuneracionPorId;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Remuneraciones.ObtenerRemuneracionPorId;

namespace Emplaniapp.LogicaDeNegocio.Remuneraciones.ObtenerRemuneracionPorId
{
    public class ObtenerRemuneracionPorIdLN : IObtenerRemuneracionPorIdLN
    {
        private IObtenerRemuneracionPorIdAD _obtenerRemuneracionPorIdAD;

        public ObtenerRemuneracionPorIdLN()
        {
            _obtenerRemuneracionPorIdAD = new ObtenerRemuneracionPorIdAD();
        }

        public RemuneracionDto ObtenerPorId(int idRemuneracion)
        {
            return _obtenerRemuneracionPorIdAD.ObtenerPorId(idRemuneracion);
        }
    }
}
