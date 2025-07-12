using System;
using System.Collections.Generic;
using System.Linq;
using Emplaniapp.Abstracciones.InterfacesAD.Tipo_Retencion;
using Emplaniapp.Abstracciones.InterfacesParaUI.TipoRetencion;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Tipo_Retencion;

namespace Emplaniapp.LogicaDeNegocio.Tipo_Retencion
{
    public class ListarTipoRetencionLN : IListarTipoRetencionLN
    {
        private readonly IListarTipoRetencionAD _repo;

        public ListarTipoRetencionLN(IListarTipoRetencionAD repo)
            => _repo = repo ?? throw new ArgumentNullException(nameof(repo));

        public ListarTipoRetencionLN()
            : this(new ListarTipoRetencionAD())
        { }

        public List<TipoRetencionDto> Listar()
            => _repo.Listar()
                    .Select(e => new TipoRetencionDto
                    {
                        Id = e.Id,
                        nombreTipoRetencion = e.nombreTipoRetencion,
                        porcentajeRetencion = e.porcentajeRetencion,
                        idEstado = e.idEstado
                    })
                    .ToList();
    }
}
