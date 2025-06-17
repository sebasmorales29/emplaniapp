using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.Abstracciones.ModelosAD;

namespace Emplaniapp.AccesoADatos
{
    public class DatosPersonalesAD : IDatosPersonalesAD
    {
        public EmpleadoDto ObtenerEmpleadoPorId(int idEmpleado)
        {
            using (var contexto = new Contexto())
            {
                var empleado = contexto.Empleados.FirstOrDefault(e => e.idEmpleado == idEmpleado);
                if (empleado == null) return null;

                // Obtener información relacionada
                var cargo = contexto.Cargos.FirstOrDefault(c => c.idCargo == empleado.idCargo);
                var estado = contexto.Estado.FirstOrDefault(e => e.idEstado == empleado.idEstado);

                return new EmpleadoDto
                {
                    idEmpleado = empleado.idEmpleado,
                    nombre = empleado.nombre,
                    segundoNombre = empleado.segundoNombre,
                    primerApellido = empleado.primerApellido,
                    segundoApellido = empleado.segundoApellido,
                    fechaNacimiento = empleado.fechaNacimiento,
                    cedula = empleado.cedula,
                    numeroTelefonico = empleado.numeroTelefonico,
                    correoInstitucional = empleado.correoInstitucional,
                    idDireccion = empleado.idDireccion,
                    idCargo = empleado.idCargo,
                    nombreCargo = cargo?.nombreCargo ?? "Sin cargo",
                    fechaContratacion = empleado.fechaContratacion,
                    fechaSalida = empleado.fechaSalida,
                    periocidadPago = empleado.periocidadPago,
                    salarioDiario = empleado.salarioDiario,
                    salarioAprobado = empleado.salarioAprobado,
                    salarioPorMinuto = empleado.salarioPorMinuto,
                    salarioPoHora = empleado.salarioPoHora,
                    salarioPorHoraExtra = empleado.salarioPorHoraExtra,
                    idMoneda = empleado.idTipoMoneda,
                    nombreMoneda = "Colones", // Por ahora hardcodeado, después implementar tabla TipoMoneda
                    cuentaIBAN = empleado.cuentaIBAN,
                    idBanco = empleado.idBanco,
                    nombreBanco = "BN", // Por ahora hardcodeado, después implementar tabla Bancos
                    idEstado = empleado.idEstado,
                    nombreEstado = estado?.nombreEstado ?? "Sin estado"
                };
            }
        }

        public bool CrearEmpleado(EmpleadoDto empleadoDto)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Iniciando CrearEmpleado en AccesoADatos");
                
                using (var contexto = new Contexto())
                {
                    System.Diagnostics.Debug.WriteLine("Contexto creado, la base de datos generará automáticamente el ID");
                    
                    // NO asignar ID manualmente cuando la columna es IDENTITY
                    // La base de datos genera automáticamente el siguiente ID disponible

                    // Calcular salarios basados en la periodicidad
                    decimal salarioDiario = 0;
                    if (empleadoDto.periocidadPago == "Quincenal")
                    {
                        salarioDiario = empleadoDto.salarioAprobado / 15;
                    }
                    else if (empleadoDto.periocidadPago == "Mensual")
                    {
                        salarioDiario = empleadoDto.salarioAprobado / 30;
                    }

                    decimal salarioPorHora = salarioDiario / 8;
                    decimal salarioPorMinuto = salarioPorHora / 60;
                    decimal salarioPorHoraExtra = salarioPorHora * 1.5m;

                    System.Diagnostics.Debug.WriteLine("Salarios calculados - Diario: " + salarioDiario);

                    var nuevoEmpleado = new Empleado
                    {
                        // Asignar el IdNetUser que viene desde el controlador
                        IdNetUser = empleadoDto.IdNetUser,

                        // NO asignar idEmpleado - se genera automáticamente
                        nombre = empleadoDto.nombre,
                        segundoNombre = empleadoDto.segundoNombre,
                        primerApellido = empleadoDto.primerApellido,
                        segundoApellido = empleadoDto.segundoApellido,
                        fechaNacimiento = empleadoDto.fechaNacimiento,
                        cedula = empleadoDto.cedula,
                        numeroTelefonico = empleadoDto.numeroTelefonico,
                        correoInstitucional = empleadoDto.correoInstitucional,
                        idDireccion = empleadoDto.idDireccion,
                        idCargo = empleadoDto.idCargo,
                        fechaContratacion = empleadoDto.fechaContratacion,
                        fechaSalida = empleadoDto.fechaSalida,
                        periocidadPago = empleadoDto.periocidadPago,
                        salarioDiario = salarioDiario,
                        salarioAprobado = empleadoDto.salarioAprobado,
                        salarioPorMinuto = salarioPorMinuto,
                        salarioPoHora = salarioPorHora,
                        salarioPorHoraExtra = salarioPorHoraExtra,
                        idTipoMoneda = empleadoDto.idMoneda,
                        cuentaIBAN = empleadoDto.cuentaIBAN,
                        idBanco = empleadoDto.idBanco,
                        idEstado = empleadoDto.idEstado
                    };

                    System.Diagnostics.Debug.WriteLine("=== VALORES DEL NUEVO EMPLEADO ===");
                    System.Diagnostics.Debug.WriteLine("nombre: " + nuevoEmpleado.nombre);
                    System.Diagnostics.Debug.WriteLine("idDireccion: " + nuevoEmpleado.idDireccion);
                    System.Diagnostics.Debug.WriteLine("idCargo: " + nuevoEmpleado.idCargo);
                    System.Diagnostics.Debug.WriteLine("idTipoMoneda: " + nuevoEmpleado.idTipoMoneda);
                    System.Diagnostics.Debug.WriteLine("idBanco: " + nuevoEmpleado.idBanco);
                    System.Diagnostics.Debug.WriteLine("idEstado: " + nuevoEmpleado.idEstado);
                    System.Diagnostics.Debug.WriteLine("cedula: " + nuevoEmpleado.cedula);
                    System.Diagnostics.Debug.WriteLine("=== FIN VALORES ===");

                    System.Diagnostics.Debug.WriteLine("Empleado creado, agregando al contexto");

                    contexto.Empleados.Add(nuevoEmpleado);
                    
                    System.Diagnostics.Debug.WriteLine("Guardando cambios...");
                    contexto.SaveChanges();
                    
                    System.Diagnostics.Debug.WriteLine("Empleado guardado exitosamente");
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en CrearEmpleado AccesoADatos: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Stack trace: " + ex.StackTrace);
                
                // Recursivamente mostrar todas las excepciones internas
                Exception innerEx = ex.InnerException;
                int nivel = 1;
                while (innerEx != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner exception nivel {nivel}: " + innerEx.Message);
                    System.Diagnostics.Debug.WriteLine($"Inner exception tipo {nivel}: " + innerEx.GetType().Name);
                    if (innerEx.InnerException == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Excepción más interna: " + innerEx.ToString());
                    }
                    innerEx = innerEx.InnerException;
                    nivel++;
                }
                return false;
            }
        }

        public bool ActualizarDatosPersonales(EmpleadoDto empleadoDto)
        {
            try
            {
                using (var contexto = new Contexto())
                {
                    var empleado = contexto.Empleados.FirstOrDefault(e => e.idEmpleado == empleadoDto.idEmpleado);
                    if (empleado == null) return false;

                    empleado.nombre = empleadoDto.nombre;
                    empleado.segundoNombre = empleadoDto.segundoNombre;
                    empleado.primerApellido = empleadoDto.primerApellido;
                    empleado.segundoApellido = empleadoDto.segundoApellido ?? "";
                    empleado.fechaNacimiento = empleadoDto.fechaNacimiento;
                    empleado.cedula = empleadoDto.cedula;
                    empleado.numeroTelefonico = empleadoDto.numeroTelefonico;
                    empleado.correoInstitucional = empleadoDto.correoInstitucional;

                    contexto.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool ActualizarDatosLaborales(int idEmpleado, int idCargo, DateTime fechaContratacion, DateTime? fechaSalida)
        {
            try
            {
                using (var contexto = new Contexto())
                {
                    var empleado = contexto.Empleados.FirstOrDefault(e => e.idEmpleado == idEmpleado);
                    if (empleado == null) return false;

                    empleado.idCargo = idCargo;
                    empleado.fechaContratacion = fechaContratacion;
                    empleado.fechaSalida = fechaSalida;

                    contexto.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool ActualizarDatosFinancieros(int idEmpleado, decimal salarioAprobado, decimal salarioDiario, 
            string periocidadPago, int idTipoMoneda, string cuentaIBAN, int idBanco)
        {
            try
            {
                using (var contexto = new Contexto())
                {
                    var empleado = contexto.Empleados.FirstOrDefault(e => e.idEmpleado == idEmpleado);
                    if (empleado == null) return false;

                    empleado.salarioAprobado = salarioAprobado;
                    empleado.salarioDiario = salarioDiario;
                    empleado.periocidadPago = periocidadPago;
                    empleado.idTipoMoneda = idTipoMoneda;
                    empleado.cuentaIBAN = cuentaIBAN;
                    empleado.idBanco = idBanco;

                    // Recalcular salarios basados en el salario aprobado
                    if (periocidadPago == "Quincenal")
                    {
                        empleado.salarioDiario = salarioAprobado / 15;
                    }
                    else if (periocidadPago == "Mensual")
                    {
                        empleado.salarioDiario = salarioAprobado / 30;
                    }

                    empleado.salarioPoHora = empleado.salarioDiario / 8;
                    empleado.salarioPorMinuto = empleado.salarioPoHora / 60;
                    empleado.salarioPorHoraExtra = empleado.salarioPoHora * 1.5m;

                    contexto.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public List<CargoDto> ObtenerCargos()
        {
            using (var contexto = new Contexto())
            {
                return contexto.Cargos.Select(c => new CargoDto
                {
                    idCargo = c.idCargo,
                    nombreCargo = c.nombreCargo
                }).ToList();
            }
        }

        public List<MonedaDto> ObtenerTiposMoneda()
        {
            // Por ahora retornamos datos hardcodeados
            return new List<MonedaDto>
            {
                new MonedaDto { idMoneda = 1, nombreMoneda = "Colones" },
                new MonedaDto { idMoneda = 2, nombreMoneda = "Dólares" }
            };
        }

        public List<BancoDto> ObtenerBancos()
        {
            using (var contexto = new Contexto())
            {
                // Por ahora retornamos datos hardcodeados
                return new List<BancoDto>
                {
                    new BancoDto { idBanco = 1, nombreBanco = "Banco Nacional" },
                    new BancoDto { idBanco = 2, nombreBanco = "Banco de Costa Rica" },
                    new BancoDto { idBanco = 3, nombreBanco = "BAC San José" }
                };
            }
        }
    }
} 