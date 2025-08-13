using Emplaniapp.Abstracciones.InterfacesAD.Empleado.ModificarEstado;
using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.ModificarEstado;
using Emplaniapp.AccesoADatos.Empleado.modificarEstado;

namespace Emplaniapp.LogicaDeNegocio.Empleado.ModificarEstado
{
    public class modificarEstadoLN : IModificarEstadoLN
    {
        private IModificarEstadoAD _modificarEstadoAD;

        public modificarEstadoLN()
        {
            _modificarEstadoAD = new modificarEstadoAD();
        }

        public bool CambiarEstadoEmpleado(int idEmpleado, int idEstado)
        {
            return _modificarEstadoAD.CambiarEstadoEmpleado(idEmpleado, idEstado);
        }
    }
}
