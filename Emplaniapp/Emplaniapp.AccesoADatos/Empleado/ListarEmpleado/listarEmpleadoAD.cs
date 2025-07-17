using System.Collections.Generic;
using System.Linq;
using Emplaniapp.Abstracciones.InterfacesAD.Empleado.ListarEmpleado;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.AccesoADatos.Empleado.listarEmpleado
{
    public class listarEmpleadoAD: IListarEmpleadoAD
    {
        private Contexto _contexto;

        public listarEmpleadoAD()
        {
            _contexto = new Contexto();
        }
        public List<EmpleadoDto> ObtenerEmpleados()
        {
            var empleadosRaw = (from emp in _contexto.Empleados
                                join estado in _contexto.Estado on emp.idEstado equals estado.idEstado
                                join cargo in _contexto.Cargos on emp.idCargo equals cargo.idCargo
                                join banco in _contexto.Bancos on emp.idBanco equals banco.idBanco
                                join moneda in _contexto.TipoMoneda on emp.idTipoMoneda equals moneda.idTipoMoneda
                                join dir in _contexto.Direccion on emp.idDireccion equals dir.idDireccion
                                join prov in _contexto.Provincia on dir.idProvincia equals prov.idProvincia
                                join cant in _contexto.Canton on dir.idCanton equals cant.idCanton
                                join dist in _contexto.Distrito on dir.idDistrito equals dist.idDistrito
                                join ca in _contexto.Calle on dir.idCalle equals ca.idCalle
                                join user in _contexto.Users on emp.IdNetUser equals user.Id into userGroup
                                from user in userGroup.DefaultIfEmpty() 
                                join userRole in _contexto.Set<Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole>() on user.Id equals userRole.UserId into userRoleGroup
                                from userRole in userRoleGroup.DefaultIfEmpty()
                                join role in _contexto.Roles on userRole.RoleId equals role.Id into roleGroup
                                from role in roleGroup.DefaultIfEmpty()
                                orderby emp.primerApellido, emp.segundoApellido, emp.nombre
                                select new
                                {
                                    emp.idEmpleado,
                                    emp.nombre,
                                    emp.segundoNombre,
                                    emp.primerApellido,
                                    emp.segundoApellido,
                                    emp.cedula,
                                    emp.fechaNacimiento,
                                    emp.numeroTelefonico,
                                    emp.correoInstitucional,
                                    emp.direccionFisica,
                                    emp.fechaContratacion,
                                    emp.fechaSalida,
                                    emp.periocidadPago,
                                    emp.salarioAprobado,
                                    emp.idEstado,
                                    estado.nombreEstado,
                                    emp.idCargo,
                                    cargo.nombreCargo,
                                    emp.idBanco,
                                    banco.nombreBanco,
                                    emp.idTipoMoneda,
                                    moneda.nombreMoneda,
                                    emp.cuentaIBAN,
                                    direccion = new
                                    {
                                        prov.nombreProvincia,
                                        cant.nombreCanton,
                                        dist.nombreDistrito,
                                        ca.nombreCalle
                                    },
                                    Role = role.Name
                                }).ToList(); // Aquí termina la ejecución en SQL

            // Agrupar en memoria para consolidar empleados con múltiples roles
            var empleados = empleadosRaw
                .GroupBy(emp => emp.idEmpleado)
                .Select(g =>
                {
                    var first = g.First();
                    var roles = g.Select(emp => emp.Role).Where(r => r != null).Distinct();
                    return new EmpleadoDto
                    {
                        idEmpleado = first.idEmpleado,
                        nombre = first.nombre,
                        segundoNombre = first.segundoNombre ?? string.Empty,
                        primerApellido = first.primerApellido,
                        segundoApellido = first.segundoApellido,
                        cedula = first.cedula,
                        fechaNacimiento = first.fechaNacimiento,
                        numeroTelefonico = first.numeroTelefonico,
                        correoInstitucional = first.correoInstitucional,
                        direccionFisica = first.direccionFisica,

                        idEstado = first.idEstado,
                        nombreEstado = first.nombreEstado,

                        idCargo = first.idCargo,
                        nombreCargo = first.nombreCargo,

                        salarioAprobado = first.salarioAprobado,
                        periocidadPago = first.periocidadPago,
                        nombreMoneda = first.nombreMoneda,
                        cuentaIBAN = first.cuentaIBAN,
                        idBanco = first.idBanco,
                        nombreBanco = first.nombreBanco,

                        direccionCompleta = $"{first.direccion.nombreProvincia}, {first.direccion.nombreCanton}, {first.direccion.nombreDistrito}, {first.direccion.nombreCalle}",

                        fechaContratacion = first.fechaContratacion,
                        fechaSalida = first.fechaSalida,
                        Role = roles.Any() ? string.Join(", ", roles) : "Sin rol"
                    };
                }).ToList();

            return empleados;
        }
    }
}
