using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Monedas.ListarMonedas;
using Emplaniapp.Abstracciones.InterfacesParaUI.Monedas.ListarMonedas;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Monedas.ListarMonedas;

namespace Emplaniapp.LogicaDeNegocio.Monedas.ListarMonedas
{
    public class listarMonedasLN : IlistarMonedasLN
    {
        private IListarMonedasAD _listarMonedasAD;

        public listarMonedasLN()
        {
            _listarMonedasAD = new listarMonedasAD();
        }

        public List<MonedaDto> ObtenerMonedas()
        {
            return _listarMonedasAD.ObtenerMonedas();
        }
    }
}
