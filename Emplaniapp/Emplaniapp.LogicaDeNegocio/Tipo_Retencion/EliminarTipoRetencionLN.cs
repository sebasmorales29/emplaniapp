// Emplaniapp.LogicaDeNegocio.Tipo_Retencion/EliminarTipoRetencionLN.cs
using Emplaniapp.Abstracciones.InterfacesAD.Tipo_Retencion;
using Emplaniapp.Abstracciones.InterfacesParaUI.TipoRetencion;
using Emplaniapp.AccesoADatos.Tipo_Retencion;

namespace Emplaniapp.LogicaDeNegocio.Tipo_Retencion
{
    public class EliminarTipoRetencionLN : IEliminarTipoRetencionLN
    {
        private readonly IEliminarTipoRetencionAD _repo;

        public EliminarTipoRetencionLN()
            : this(new EliminarTipoRetencionAD())
        { }

        public EliminarTipoRetencionLN(IEliminarTipoRetencionAD repo) => _repo = repo;

        public int Eliminar(int idTipoRetencion) => _repo.Eliminar(idTipoRetencion);
    }
}
