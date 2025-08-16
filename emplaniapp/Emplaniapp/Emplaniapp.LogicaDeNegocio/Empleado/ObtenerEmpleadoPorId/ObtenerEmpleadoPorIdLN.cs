using System;
using Emplaniapp.Abstracciones.InterfacesAD.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Empleado.ObtenerEmpleadoPorId;

namespace Emplaniapp.LogicaDeNegocio.Empleado.ObtenerEmpleadoPorId
{
    public class ObtenerEmpleadoPorIdLN : IObtenerEmpleadoPorIdLN
    {
        private readonly IObtenerEmpleadoPorIdAD _repo;

        // Inyección
        public ObtenerEmpleadoPorIdLN(IObtenerEmpleadoPorIdAD repo)
            => _repo = repo ?? throw new ArgumentNullException(nameof(repo));

        // Constructor por defecto
        public ObtenerEmpleadoPorIdLN()
            : this(new ObtenerEmpleadoPorIdAD())
        { }

        public EmpleadoDto ObtenerEmpleadoPorId(int idEmpleado)
            => _repo.ObtenerEmpleadoPorId(idEmpleado);
    }
}
