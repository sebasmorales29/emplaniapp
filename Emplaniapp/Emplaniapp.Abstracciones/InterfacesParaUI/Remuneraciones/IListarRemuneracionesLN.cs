using Emplaniapp.Abstracciones.ModelosParaUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones
{
    public interface IListarRemuneracionesLN
    {
        List<RemuneracionDto> Listar(int id);
        RemuneracionDto ObtenerPorId(int id);
    }
}
