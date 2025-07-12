using System;
using Emplaniapp.Abstracciones.InterfacesAD.Tipo_Retencion;
using Emplaniapp.Abstracciones.InterfacesParaUI.TipoRetencion;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Tipo_Retencion;

namespace Emplaniapp.LogicaDeNegocio.Tipo_Retencion
{
    public class ObtenerIdTipoRetencionLN : IObtenerIdTipoRetencionLN
    {
        private readonly IObtenerIdTipoRetencionAD _repo;

        public ObtenerIdTipoRetencionLN(IObtenerIdTipoRetencionAD repo)
            => _repo = repo ?? throw new ArgumentNullException(nameof(repo));

        public ObtenerIdTipoRetencionLN()
            : this(new ObtenerIdTipoRetencionAD())
        { }

        public TipoRetencionDto Obtener(int idTipoRetencion)
        {
            var e = _repo.Obtener(idTipoRetencion);
            if (e == null) return null;
            return new TipoRetencionDto
            {
                Id = e.Id,
                nombreTipoRetencion = e.nombreTipoRetencion,
                porcentajeRetencion = e.porcentajeRetencion,
                idEstado = e.idEstado
            };
        }
    }
}
