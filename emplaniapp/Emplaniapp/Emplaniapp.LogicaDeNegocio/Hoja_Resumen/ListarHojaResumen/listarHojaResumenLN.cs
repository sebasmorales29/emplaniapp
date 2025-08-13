using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Hoja_Resumen.ListarHojaResumen;
using Emplaniapp.Abstracciones.InterfacesParaUI.Hoja_Resumen.ListarHojaResumen;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Hoja_Resumen;

namespace Emplaniapp.LogicaDeNegocio.Hoja_Resumen.ListarHojaResumen
{
    public class listarHojaResumenLN: IlistarHojaResumenLN
    {
        private IListarHojaResumenAD _listarHojaResumenAD;

        public listarHojaResumenLN()
        {
            _listarHojaResumenAD =  new listarHojaResumenAD();
        }
        public List<HojaResumenDto> ObtenerHojasResumen()
        {
            return _listarHojaResumenAD.ObtenerHojasResumen();
        }
    }
}
