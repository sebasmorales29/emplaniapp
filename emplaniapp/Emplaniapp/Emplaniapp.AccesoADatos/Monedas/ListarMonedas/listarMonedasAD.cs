using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Monedas.ListarMonedas;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.AccesoADatos.Monedas.ListarMonedas
{
    public class listarMonedasAD : IListarMonedasAD
    {
        private Contexto _contexto;

        public listarMonedasAD()
        {
            _contexto = new Contexto();
        }
        public List<MonedaDto> ObtenerMonedas()
        {
            return _contexto.TipoMoneda
                .Select(m => new MonedaDto
                {
                    idMoneda = m.idTipoMoneda,
                    nombreMoneda = m.nombreMoneda
                }).ToList();
        }
    }
}
