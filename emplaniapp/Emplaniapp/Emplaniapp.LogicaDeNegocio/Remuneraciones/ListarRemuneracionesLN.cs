using Emplaniapp.Abstracciones.InterfacesAD.Remuneraciones;
using Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Remuneraciones;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.LogicaDeNegocio.Remuneraciones
{
    public class ListarRemuneracionesLN : IListarRemuneracionesLN
    {
        IListarRemuneracionesAD _listar;

        public ListarRemuneracionesLN()
        {
            _listar = new ListarRemuneracionesAD();
        }


        public List<RemuneracionDto> Listar(int id)
        {
            List<RemuneracionDto> ListaRemuneraciones = _listar.Listar(id);
            return ListaRemuneraciones;
        }

        public RemuneracionDto ObtenerPorId(int id)
        {
            return _listar.ObtenerPorId(id);
        }
    }
}
