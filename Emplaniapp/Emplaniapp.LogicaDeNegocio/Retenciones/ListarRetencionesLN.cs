﻿using System.Collections.Generic;
using Emplaniapp.Abstracciones.InterfacesAD.Retenciones;
using Emplaniapp.Abstracciones.InterfacesParaUI.Retenciones;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Retenciones;

namespace Emplaniapp.LogicaDeNegocio.Retenciones
{
    public class ListarRetencionesLN : IListarRetencionesLN
    {
        private readonly IListarRetencionesAD _repo;

        public ListarRetencionesLN()
            : this(new ListarRetencionesAD())
        { }

        public ListarRetencionesLN(IListarRetencionesAD repo) => _repo = repo;

        public List<RetencionDto> Listar(int idEmpleado) => _repo.Listar(idEmpleado);
    }
}
