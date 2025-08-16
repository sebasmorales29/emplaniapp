using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.Abstracciones.InterfacesAD.Empleado.AgregarEmpleado
{
    public interface IAgregarEmpleadoAD
    {
        bool CrearEmpleado(EmpleadoDto empleado);
    }
}
