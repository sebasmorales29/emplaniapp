using System;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Tipo_Retencion;
using Emplaniapp.Abstracciones.InterfacesParaUI.TipoRetencion;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.AccesoADatos.Tipo_Retencion;

namespace Emplaniapp.LogicaDeNegocio.Tipo_Retencion
{
    public class AgregarTipoRetencionLN : IAgregarTipoRetencionLN
    {
        private readonly IAgregarTipoRetencionAD _repo;

        public AgregarTipoRetencionLN(IAgregarTipoRetencionAD repo)
            => _repo = repo ?? throw new ArgumentNullException(nameof(repo));

        public AgregarTipoRetencionLN()
            : this(new AgregarTipoRetencionAD())
        { }

        public async Task<int> Guardar(TipoRetencionDto dto)
        {
            var entidad = new TipoRetencion
            {
                nombreTipoRetencion = dto.nombreTipoRetencion,
                porcentajeRetencion = dto.porcentajeRetencion,
                idEstado = dto.idEstado
            };
            return await _repo.AgregarAsync(entidad);
        }
    }
}
