using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Cargos.ListarCargos;
using Emplaniapp.Abstracciones.InterfacesParaUI.Cargos.ListarCargos;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Cargos.listarCargos;

namespace Emplaniapp.LogicaDeNegocio.Cargos.ListarCargos
{
    public class listarCargosLN : IListarCargosLN
    {
        private IListarCargosAD _listarCargosAD;
        public listarCargosLN()
        {
            _listarCargosAD = new listarCargosAD();
        }
        public List<CargoDto> ObtenerCargos()
        {
            return _listarCargosAD.ObtenerCargos();
        }
    }
}
