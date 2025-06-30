// Emplaniapp.LogicaDeNegocio.Tipo_Retencion/EditarTipoRetencionLN.cs
using Emplaniapp.Abstracciones.InterfacesAD.Tipo_Retencion;
using Emplaniapp.Abstracciones.InterfacesParaUI.TipoRetencion;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.AccesoADatos.Tipo_Retencion;

namespace Emplaniapp.LogicaDeNegocio.Tipo_Retencion
{
    public class EditarTipoRetencionLN : IEditarTipoRetencionLN
    {
        private readonly IEditarTipoRetencionAD _repo;

        public EditarTipoRetencionLN()
            : this(new EditarTipoRetencionAD())
        { }

        public EditarTipoRetencionLN(IEditarTipoRetencionAD repo) => _repo = repo;

        public int Editar(TipoRetencionDto dto)
        {
            var entidad = new TipoRetencion
            {
                Id = dto.Id,
                nombreTipoRetencion = dto.nombreTipoRetencion,
                porcentajeRetencion = dto.porcentajeRetencion,
                idEstado = dto.idEstado
            };
            return _repo.Editar(entidad);
        }
    }
}
