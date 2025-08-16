using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.Abstracciones.InterfacesParaUI.Retenciones
{
    public interface IListarRetencionesLN
    {
        List<RetencionDto> Listar(int idEmpleado);
    }
}