using System;
using Emplaniapp.Abstracciones.InterfacesAD.Retenciones;
using Emplaniapp.Abstracciones.InterfacesParaUI.Retenciones;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.AccesoADatos.Retenciones;

namespace Emplaniapp.LogicaDeNegocio.Retenciones
{
    public class AgregarRetencionLN : IAgregarRetencionLN
    {
        private readonly IAgregarRetencionAD _repo;

        public AgregarRetencionLN(IAgregarRetencionAD repo)
            => _repo = repo ?? throw new ArgumentNullException(nameof(repo));

        public AgregarRetencionLN()
            : this(new AgregarRetencionAD())
        { }

        public void AgregarRetencion(RetencionCrearDto dto)
        {
            var entidad = new Retencion
            {
                idEmpleado = dto.idEmpleado,
                idTipoRetencion = dto.idTipoRetencio,
                rebajo = dto.rebajo,
                fechaRetencion = dto.fechaRetencio,
                idEstado = 1
            };
            _repo.Agregar(entidad);
        }
    }
}
