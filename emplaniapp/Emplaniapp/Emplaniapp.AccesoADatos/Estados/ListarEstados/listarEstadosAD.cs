using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Estados.ListarEstados;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.AccesoADatos.Estados.listarEstados
{
    public class listarEstadosAD : IListarEstadosAD
    {
        private Contexto _contexto;

        public listarEstadosAD()
        {
            _contexto = new Contexto();
        }
        public List<EstadoDto> ObtenerEstados()
        {
            return _contexto.Estado
                .Select(e => new EstadoDto
                {
                    idEstado = e.idEstado,
                    nombreEstado = e.nombreEstado
                }).ToList();
        }
    }
}
