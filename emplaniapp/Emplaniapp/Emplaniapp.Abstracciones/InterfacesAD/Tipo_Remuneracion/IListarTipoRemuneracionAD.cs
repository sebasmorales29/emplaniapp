using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.Abstracciones.ModelosParaUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.InterfacesAD.Tipo_Remuneracion
{
    public interface IListarTipoRemuneracionAD
    {
        List<TipoRemuneracion> Listar();
        List<TipoRemuneracionDto> ObtenerTipoRemuneracion();
    }
}
