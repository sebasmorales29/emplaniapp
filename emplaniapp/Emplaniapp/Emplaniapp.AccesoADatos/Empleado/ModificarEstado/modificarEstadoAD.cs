using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Empleado.ModificarEstado;

namespace Emplaniapp.AccesoADatos.Empleado.modificarEstado
{
    public class modificarEstadoAD : IModificarEstadoAD
    {
        private Contexto _contexto;

        public modificarEstadoAD()
        {
            _contexto = new Contexto();
        }
        public bool CambiarEstadoEmpleado(int idEmpleado, int idEstado)
        {
            try
            {
                var empleado = _contexto.Empleados.FirstOrDefault(e => e.idEmpleado == idEmpleado);
                if (empleado != null)
                {
                    empleado.idEstado = idEstado;
                    _contexto.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
