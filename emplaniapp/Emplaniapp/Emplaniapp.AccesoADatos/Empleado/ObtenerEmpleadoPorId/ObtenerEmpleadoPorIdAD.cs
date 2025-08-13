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
                        // Identificador
                        idEmpleado = emp.idEmpleado,
                        idEstado = emp.idEstado,
                        nombreEstado = estado.nombreEstado,

                        //Datos personales
                        nombre = emp.nombre,
                        segundoNombre = emp.segundoNombre,
                        primerApellido = emp.primerApellido,
                        segundoApellido = emp.segundoApellido,
                        fechaNacimiento = emp.fechaNacimiento,
                        cedula = emp.cedula,
                        numeroTelefonico = emp.numeroTelefonico,
                        correoInstitucional= emp.correoInstitucional,
                        //idDireccion = emp.idDireccion,

                        // Datos Laborales
                        idCargo = emp.idCargo,
                        nombreCargo = cargo.nombreCargo,
                        fechaContratacion = emp.fechaContratacion,
                        fechaSalida = emp.fechaSalida,
                        
                        // Salario
                        salarioAprobado = emp.salarioAprobado,
                        salarioDiario = emp.salarioDiario,
                        salarioPoHora = emp.salarioPoHora,
                        salarioPorMinuto = emp.salarioPorMinuto,
                        salarioPorHoraExtra = emp.salarioPorHoraExtra,
                        periocidadPago = emp.periocidadPago,
                        idMoneda = emp.idTipoMoneda,
                        cuentaIBAN = emp.cuentaIBAN,
                        idBanco = emp.idBanco
                        

                    }).FirstOrDefault();
        }
    }
}
