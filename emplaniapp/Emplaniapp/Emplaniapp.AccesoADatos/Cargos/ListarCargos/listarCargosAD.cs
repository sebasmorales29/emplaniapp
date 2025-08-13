using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Cargos.ListarCargos;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.AccesoADatos.Cargos.listarCargos
{
    public class listarCargosAD : IListarCargosAD
    {
        private Contexto _contexto;

        public listarCargosAD()
        {
            _contexto = new Contexto();
        }

        public List<CargoDto> ObtenerCargos()
        {
            return _contexto.Cargos
                .Select(p => new CargoDto
                {
                    idCargo = p.idCargo,
                    nombreCargo = p.nombreCargo
                }).ToList();
        }
    }
}
