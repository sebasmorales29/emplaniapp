using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Estados.ListarEstados;
using Emplaniapp.Abstracciones.InterfacesParaUI.Estados.ListarEstados;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Estados.listarEstados;

namespace Emplaniapp.LogicaDeNegocio.Estados.ListarEstados
{
    public class listarEstadosLN : IListarEstadosLN
    {
        private IListarEstadosAD _listarEstadosAD;
        public listarEstadosLN()
        {
            _listarEstadosAD = new listarEstadosAD();
        }
        public List<EstadoDto> ObtenerEstados()
        {
            return _listarEstadosAD.ObtenerEstados();
        }
    }
}
