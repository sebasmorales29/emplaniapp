using System;
using Emplaniapp.Abstracciones.InterfacesAD.Retenciones;
using Emplaniapp.Abstracciones.InterfacesParaUI.Retenciones;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.AccesoADatos.Retenciones;

namespace Emplaniapp.LogicaDeNegocio.Retenciones
{
    public class EditarRetencionLN : IEditarRetencionLN
    {
        private readonly IEditarRetencionAD _repo;

        public EditarRetencionLN(IEditarRetencionAD repo)
            => _repo = repo ?? throw new ArgumentNullException(nameof(repo));

        public EditarRetencionLN()
            : this(new EditarRetencionAD())
        { }

        public void EditarRetencion(RetencionEditarDto dto)
        {
            var entidad = new Retencion
            {
                idRetencion = dto.idRetencion,
                idTipoRetencion = dto.idTipoRetencio,
                rebajo = dto.rebajo,
                fechaRetencion = dto.fechaRetencio
            };
            _repo.Editar(entidad);
        }
    }
}
