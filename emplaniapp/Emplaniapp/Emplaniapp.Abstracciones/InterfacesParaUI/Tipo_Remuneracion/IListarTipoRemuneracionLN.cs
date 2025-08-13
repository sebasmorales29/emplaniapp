using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.Abstracciones.ModelosParaUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.InterfacesParaUI.Tipo_Remuneracion
{
    public interface IListarTipoRemuneracionLN
    {
        List<TipoRemuneracion> ListarTipoRemuneracion();
        List<TipoRemuneracionDto> Listar();
        List<TipoRemuneracionDto> ObtenerTipoRemuneracion();
    }
}
