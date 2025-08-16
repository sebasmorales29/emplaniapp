using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Bancos.listarBancos;
using Emplaniapp.Abstracciones.InterfacesParaUI.Bancos.ListarBancos;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Bancos.ListarBancos;

namespace Emplaniapp.LogicaDeNegocio.Bancos.ListarBancos
{
    public class listarBancosLN : IListarBancosLN
    {
        private IListarBancosAD _listarBancosAD;
        public listarBancosLN()
        {
            _listarBancosAD = new listarBancosAD();
        }

        public List<BancoDto> ObtenerBancos()
        {
            return _listarBancosAD.ObtenerBancos();
        }
    }
}
