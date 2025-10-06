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
            return ObtenerEmpleados(null);
        }

        public List<EmpleadoDto> ObtenerEmpleados(string usuarioActualId = null)
        {
            // Primero obtenemos los datos básicos de empleados sin roles
            var empleadosBase = (from emp in _contexto.Empleados
                                join estado in _contexto.Estado on emp.idEstado equals estado.idEstado
                                join cargo in _contexto.Cargos on emp.idCargo equals cargo.idCargo
                                join banco in _contexto.Bancos on emp.idBanco equals banco.idBanco
                                join moneda in _contexto.TipoMoneda on emp.idTipoMoneda equals moneda.idTipoMoneda
                                join prov in _contexto.Provincia on emp.idProvincia equals prov.idProvincia
                                join cant in _contexto.Canton on emp.idCanton equals cant.idCanton
                                join dist in _contexto.Distrito on emp.idDistrito equals dist.idDistrito
                                join user in _contexto.Users on emp.IdNetUser equals user.Id into userGroup
                                from user in userGroup.DefaultIfEmpty() 
                                // ✨ MEJORA: Excluir el usuario actual del listado por seguridad
                                where usuarioActualId == null || emp.IdNetUser != usuarioActualId
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
                                    nombreProvincia = prov.nombreProvincia,
                                    nombreCanton = cant.nombreCanton,
                                    nombreDistrito = dist.nombreDistrito,
                                    emp.IdNetUser
                                }).ToList();

            // Obtenemos los roles agrupados por usuario
            var rolesRaw = (from userRole in _contexto.Set<Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole>()
                           join role in _contexto.Roles on userRole.RoleId equals role.Id
                           select new
                           {
                               UserId = userRole.UserId,
                               RoleName = role.Name
                           }).ToList(); // Ejecutamos la consulta primero

            // Agrupamos y concatenamos los roles en memoria
            var rolesAgrupadosPorUsuario = rolesRaw
                .GroupBy(x => x.UserId)
                .ToDictionary(g => g.Key, g => string.Join(", ", g.Select(x => x.RoleName)));

            // Convertimos la información a DTOs
            var empleados = empleadosBase.Select(emp => new EmpleadoDto
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

                direccionCompleta = $"{emp.nombreProvincia}, {emp.nombreCanton}, {emp.nombreDistrito}",

                fechaContratacion = emp.fechaContratacion,
                fechaSalida = emp.fechaSalida,
                
                // Obtener roles concatenados o "Sin rol" si no tiene
                RolesUsuario = !string.IsNullOrEmpty(emp.IdNetUser) && rolesAgrupadosPorUsuario.ContainsKey(emp.IdNetUser) 
                      ? rolesAgrupadosPorUsuario[emp.IdNetUser] 
                      : "Sin rol"
            }).ToList();

            return empleados;
        }
    }
}
