using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.ModificarEstado
{
    public interface IModificarEstadoLN
    {
        bool CambiarEstadoEmpleado(int idEmpleado, int idEstado);
    }
}
