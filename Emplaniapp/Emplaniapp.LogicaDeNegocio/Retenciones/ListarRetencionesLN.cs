using Emplaniapp.Abstracciones.InterfacesAD.Retenciones;
using Emplaniapp.Abstracciones.InterfacesParaUI.Retenciones;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Retenciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.LogicaDeNegocio.Retenciones
{
    public class ListarRetencionesLN : IListarRetencionesLN
    {
        IListarRetencionesAD _listar;

        public ListarRetencionesLN()
        {
            _listar = new ListarRetencionesAD();
        }


        public List<RetencionDto> Listar(int id)
        {
            List<RetencionDto> lista = _listar.Listar(id);
            return lista;
        }

    }
}
