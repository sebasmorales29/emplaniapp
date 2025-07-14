using System;
using Emplaniapp.Abstracciones.InterfacesAD.Retenciones;
using Emplaniapp.Abstracciones.InterfacesParaUI.Retenciones;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Retenciones;

namespace Emplaniapp.LogicaDeNegocio.Retenciones
{
    public class ObtenerRetencionPorIdLN : IObtenerRetencionPorIdLN
    {
        private readonly IObtenerRetencionPorIdAD _repo;

        public ObtenerRetencionPorIdLN(IObtenerRetencionPorIdAD repo)
            => _repo = repo ?? throw new ArgumentNullException(nameof(repo));

        public ObtenerRetencionPorIdLN()
            : this(new ObtenerRetencionPorIdAD())
        { }

        public RetencionDto Obtener(int idRetencion)
            => _repo.Obtener(idRetencion);
    }
}
