using Emplaniapp.Abstracciones.InterfacesAD.Retenciones;
using Emplaniapp.Abstracciones.InterfacesParaUI.Retenciones;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Retenciones;

namespace Emplaniapp.LogicaDeNegocio.Retenciones
{
    public class ObtenerRetencionPorIdLN : IObtenerRetencionPorIdLN
    {
        private readonly IObtenerRetencionPorIdAD _repo;

        public ObtenerRetencionPorIdLN()
            : this(new ObtenerRetencionPorIdAD())
        { }

        public ObtenerRetencionPorIdLN(IObtenerRetencionPorIdAD repo) => _repo = repo;

        public RetencionDto Obtener(int idRetencion)
        {
            return _repo.Obtener(idRetencion);
        }
    }
}
