using System;
using Emplaniapp.Abstracciones.InterfacesAD.Tipo_Retencion;
using Emplaniapp.Abstracciones.InterfacesParaUI.TipoRetencion;
using Emplaniapp.AccesoADatos.Tipo_Retencion;

namespace Emplaniapp.LogicaDeNegocio.Tipo_Retencion
{
    public class EliminarTipoRetencionLN : IEliminarTipoRetencionLN
    {
        private readonly IEliminarTipoRetencionAD _repo;

        public EliminarTipoRetencionLN(IEliminarTipoRetencionAD repo)
            => _repo = repo ?? throw new ArgumentNullException(nameof(repo));

        public EliminarTipoRetencionLN()
            : this(new EliminarTipoRetencionAD())
        { }

        public int Eliminar(int idTipoRetencion)
            => _repo.Eliminar(idTipoRetencion);
    }
}
