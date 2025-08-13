using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.Abstracciones.InterfacesAD.Estados.ListarEstados
{
    public interface IListarEstadosAD
    {
        List<EstadoDto> ObtenerEstados();
    }
}
