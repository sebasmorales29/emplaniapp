using System.Linq;
using Emplaniapp.Abstracciones.InterfacesAD.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.AccesoADatos.Empleado.ObtenerEmpleadoPorId
{
    public class ObtenerEmpleadoPorIdAD : IObtenerEmpleadoPorIdAD
    {
        private readonly Contexto _contexto;

        public ObtenerEmpleadoPorIdAD()
        {
            _contexto = new Contexto();
        }

        public EmpleadoDto ObtenerEmpleadoPorId(int idEmpleado)
        {
            return (from emp in _contexto.Empleados.AsNoTracking()
                    join estado in _contexto.Estado on emp.idEstado equals estado.idEstado
                    join cargo in _contexto.Cargos on emp.idCargo equals cargo.idCargo
                    where emp.idEmpleado == idEmpleado
                    select new EmpleadoDto
                    {
                        idEmpleado = emp.idEmpleado,
                        nombre = emp.nombre,
                        segundoNombre = emp.segundoNombre,
                        primerApellido = emp.primerApellido,
                        segundoApellido = emp.segundoApellido,
                        cedula = emp.cedula,
                        idEstado = emp.idEstado,
                        nombreEstado = estado.nombreEstado,
                        idCargo = emp.idCargo,
                        nombreCargo = cargo.nombreCargo,

                        // Se añade el salario aprobado
                        salarioAprobado = emp.salarioAprobado
                    }).FirstOrDefault();
        }
    }
}
