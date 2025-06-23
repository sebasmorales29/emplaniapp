using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Bancos.listarBancos;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.AccesoADatos.Bancos.ListarBancos
{
    public class listarBancosAD : IListarBancosAD
    {
        private Contexto _contexto;

        public listarBancosAD()
        {
            _contexto = new Contexto();
        }
        public List<BancoDto> ObtenerBancos()
        {
            return _contexto.Bancos
                .Select(b => new BancoDto
                {
                    idBanco = b.idBanco,
                    nombreBanco = b.nombreBanco
                }).ToList();
        }
    }
}
