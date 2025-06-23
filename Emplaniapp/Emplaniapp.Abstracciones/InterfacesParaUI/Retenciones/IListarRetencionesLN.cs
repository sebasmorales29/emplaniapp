using Emplaniapp.Abstracciones.ModelosParaUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.InterfacesParaUI.Retenciones
{
    public interface IListarRetencionesLN
    {
        List<RetencionDto> Listar(int id);
    }
}
