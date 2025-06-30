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

            // Ahora sí, puedes usar interpolación
            var empleados = empleadosRaw.Select(emp => new EmpleadoDto
            {
                idEmpleado = emp.idEmpleado,
                nombre = emp.nombre,
                segundoNombre = emp.segundoNombre ?? string.Empty,
                primerApellido = emp.primerApellido,
                segundoApellido = emp.segundoApellido,
                cedula = emp.cedula,
                fechaNacimiento = emp.fechaNacimiento,
                numeroTelefonico = emp.numeroTelefonico,
                correoInstitucional = emp.correoInstitucional,

                idEstado = emp.idEstado,
                nombreEstado = emp.nombreEstado,

                idCargo = emp.idCargo,
                nombreCargo = emp.nombreCargo,

                salarioAprobado = emp.salarioAprobado,
                periocidadPago = emp.periocidadPago,
                nombreMoneda = emp.nombreMoneda,
                cuentaIBAN = emp.cuentaIBAN,
                idBanco = emp.idBanco,
                nombreBanco = emp.nombreBanco,

                direccionCompleta = $"{emp.direccion.nombreProvincia}, {emp.direccion.nombreCanton}, {emp.direccion.nombreDistrito}, {emp.direccion.nombreCalle}",

                fechaContratacion = emp.fechaContratacion,
                fechaSalida = emp.fechaSalida,
                Role = emp.Role ?? "Sin rol"
            }).ToList();

            return empleados;
        }
    }
}
