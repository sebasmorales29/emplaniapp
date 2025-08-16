using System;
using Emplaniapp.Abstracciones.InterfacesAD.Retenciones;
using Emplaniapp.Abstracciones.InterfacesParaUI.Retenciones;
using Emplaniapp.AccesoADatos.Retenciones;

namespace Emplaniapp.LogicaDeNegocio.Retenciones
{
    public class EliminarRetencionLN : IEliminarRetencionLN
    {
        private readonly IEliminarRetencionAD _repo;

        public EliminarRetencionLN(IEliminarRetencionAD repo)
            => _repo = repo ?? throw new ArgumentNullException(nameof(repo));

        public EliminarRetencionLN()
            : this(new EliminarRetencionAD())
        { }

        public void EliminarRetencion(int idRetencion)
            => _repo.Eliminar(idRetencion);
    }
}
