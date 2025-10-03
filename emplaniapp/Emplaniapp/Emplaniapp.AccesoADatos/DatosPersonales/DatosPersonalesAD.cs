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
                var consulta = from emp in contexto.Empleados
                              join cargo in contexto.Cargos on emp.idCargo equals cargo.idCargo into cargoGroup
                              from cargoData in cargoGroup.DefaultIfEmpty()
                              join estado in contexto.Estado on emp.idEstado equals estado.idEstado into estadoGroup
                              from estadoData in estadoGroup.DefaultIfEmpty()
                              join tipoMoneda in contexto.TipoMoneda on emp.idTipoMoneda equals tipoMoneda.idTipoMoneda into monedaGroup
                              from monedaData in monedaGroup.DefaultIfEmpty()
                              join banco in contexto.Bancos on emp.idBanco equals banco.idBanco into bancoGroup
                              from bancoData in bancoGroup.DefaultIfEmpty()
                              // 🔥 NUEVOS JOINS PARA INFORMACIÓN GEOGRÁFICA
                              join provincia in contexto.Provincia on emp.idProvincia equals provincia.idProvincia into provinciaGroup
                              from provinciaData in provinciaGroup.DefaultIfEmpty()
                              join canton in contexto.Canton on emp.idCanton equals canton.idCanton into cantonGroup
                              from cantonData in cantonGroup.DefaultIfEmpty()
                              join distrito in contexto.Distrito on emp.idDistrito equals distrito.idDistrito into distritoGroup
                              from distritoData in distritoGroup.DefaultIfEmpty()
                              where emp.idEmpleado == idEmpleado
                              select new 
                              {
                                  Empleado = emp,
                                  NombreCargo = cargoData.nombreCargo,
                                  NombreEstado = estadoData.nombreEstado,
                                  NombreMoneda = monedaData.nombreMoneda,
                                  NombreBanco = bancoData.nombreBanco,
                                  NombreProvincia = provinciaData.nombreProvincia,
                                  NombreCanton = cantonData.nombreCanton,
                                  NombreDistrito = distritoData.nombreDistrito
                              };

                var resultado = consulta.FirstOrDefault();
                if (resultado == null) return null;

                var empleado = resultado.Empleado;

                System.Diagnostics.Debug.WriteLine($"=== ObtenerEmpleadoPorId - Con información geográfica ===");
                System.Diagnostics.Debug.WriteLine($"Empleado: {empleado.nombre} {empleado.primerApellido}");
                System.Diagnostics.Debug.WriteLine($"Provincia: {resultado.NombreProvincia ?? "No encontrada"}");
                System.Diagnostics.Debug.WriteLine($"Cantón: {resultado.NombreCanton ?? "No encontrado"}");
                System.Diagnostics.Debug.WriteLine($"Distrito: {resultado.NombreDistrito ?? "No encontrado"}");
                System.Diagnostics.Debug.WriteLine($"Dirección detallada: {empleado.direccionDetallada ?? "No especificada"}");

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
                    
                    // 🔥 CAMPOS GEOGRÁFICOS CON IDs Y NOMBRES
                    idProvincia = empleado.idProvincia,
                    nombreProvincia = resultado.NombreProvincia ?? "No especificada",
                    nombreCanton = resultado.NombreCanton ?? "No especificado",
                    nombreDistrito = resultado.NombreDistrito ?? "No especificado",
                    direccionDetallada = empleado.direccionDetallada ?? "No especificada",
                    
                    // 🔥 DIRECCIÓN COMPLETA CONCATENADA (sin calle)
                    direccionCompleta = $"{resultado.NombreProvincia ?? "Provincia no especificada"}, " +
                                       $"{resultado.NombreCanton ?? "Cantón no especificado"}, " +
                                       $"{resultado.NombreDistrito ?? "Distrito no especificado"}. " +
                                       $"{empleado.direccionDetallada ?? "Sin detalles adicionales"}",
                    
                    idCargo = empleado.idCargo,
                    nombreCargo = resultado.NombreCargo ?? "Sin cargo",
                    fechaContratacion = empleado.fechaContratacion,
                    fechaSalida = empleado.fechaSalida,
                    periocidadPago = empleado.periocidadPago,
                    salarioDiario = empleado.salarioDiario,
                    salarioAprobado = empleado.salarioAprobado,
                    salarioPorMinuto = empleado.salarioPorMinuto,
                    salarioPoHora = empleado.salarioPoHora,
                    salarioPorHoraExtra = empleado.salarioPorHoraExtra,
                    idMoneda = empleado.idTipoMoneda,
                    nombreMoneda = resultado.NombreMoneda ?? "Sin moneda",
                    cuentaIBAN = empleado.cuentaIBAN,
                    idBanco = empleado.idBanco,
                    nombreBanco = resultado.NombreBanco ?? "Sin banco",
                    idEstado = empleado.idEstado,
                    nombreEstado = resultado.NombreEstado ?? "Sin estado",
                    IdNetUser = empleado.IdNetUser
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

                    var nuevoEmpleado = new Empleados
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
                        idProvincia = empleadoDto.idProvincia,
                        idCanton = ObtenerOCrearCantonPorNombre(empleadoDto.nombreCanton, empleadoDto.idProvincia ?? 1),
                        idDistrito = ObtenerOCrearDistritoPorNombre(empleadoDto.nombreDistrito, empleadoDto.nombreCanton, empleadoDto.idProvincia ?? 1),
                        direccionDetallada = empleadoDto.direccionDetallada,
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
                System.Diagnostics.Debug.WriteLine($"=== ACCESO A DATOS - ActualizarDatosPersonales ===");
                System.Diagnostics.Debug.WriteLine($"Empleado ID: {empleadoDto.idEmpleado}");
                
                using (var contexto = new Contexto())
                {
                    var empleado = contexto.Empleados.FirstOrDefault(e => e.idEmpleado == empleadoDto.idEmpleado);
                    if (empleado == null) 
                    {
                        System.Diagnostics.Debug.WriteLine("ERROR: Empleado no encontrado en la base de datos");
                        return false;
                    }

                    System.Diagnostics.Debug.WriteLine($"Empleado encontrado. Actualizando datos...");
                    System.Diagnostics.Debug.WriteLine($"Nombre anterior: {empleado.nombre} -> Nuevo: {empleadoDto.nombre}");
                    
                    // 📋 DATOS PERSONALES BÁSICOS
                    empleado.nombre = empleadoDto.nombre;
                    empleado.segundoNombre = empleadoDto.segundoNombre;
                    empleado.primerApellido = empleadoDto.primerApellido;
                    empleado.segundoApellido = empleadoDto.segundoApellido ?? "";
                    empleado.fechaNacimiento = empleadoDto.fechaNacimiento;
                    empleado.cedula = empleadoDto.cedula;
                    empleado.numeroTelefonico = empleadoDto.numeroTelefonico;
                    empleado.correoInstitucional = empleadoDto.correoInstitucional;

                    // 🔥 DATOS DE DIRECCIÓN
                    if (empleadoDto.idProvincia.HasValue)
                    {
                        empleado.idProvincia = empleadoDto.idProvincia.Value;
                        System.Diagnostics.Debug.WriteLine($"Provincia actualizada: {empleadoDto.idProvincia.Value}");
                    }
                    
                    if (!string.IsNullOrWhiteSpace(empleadoDto.nombreCanton))
                    {
                        empleado.idCanton = ObtenerOCrearCantonPorNombre(empleadoDto.nombreCanton, empleadoDto.idProvincia ?? 1);
                        System.Diagnostics.Debug.WriteLine($"Cantón actualizado: {empleadoDto.nombreCanton} (ID: {empleado.idCanton})");
                    }
                    
                    if (!string.IsNullOrWhiteSpace(empleadoDto.nombreDistrito))
                    {
                        empleado.idDistrito = ObtenerOCrearDistritoPorNombre(empleadoDto.nombreDistrito, empleadoDto.nombreCanton, empleadoDto.idProvincia ?? 1);
                        System.Diagnostics.Debug.WriteLine($"Distrito actualizado: {empleadoDto.nombreDistrito} (ID: {empleado.idDistrito})");
                    }
                    

                    
                    if (!string.IsNullOrWhiteSpace(empleadoDto.direccionDetallada))
                    {
                        empleado.direccionDetallada = empleadoDto.direccionDetallada;
                        System.Diagnostics.Debug.WriteLine($"Dirección detallada actualizada: {empleadoDto.direccionDetallada}");
                    }

                    int cambios = contexto.SaveChanges();
                    System.Diagnostics.Debug.WriteLine($"Número de cambios guardados: {cambios}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR en ActualizarDatosPersonales: {ex.Message}");
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
                System.Diagnostics.Debug.WriteLine($"=== ACCESO A DATOS - ActualizarDatosFinancieros ===");
                System.Diagnostics.Debug.WriteLine($"Parámetros recibidos:");
                System.Diagnostics.Debug.WriteLine($"- idEmpleado: {idEmpleado}");
                System.Diagnostics.Debug.WriteLine($"- salarioAprobado: {salarioAprobado}");
                System.Diagnostics.Debug.WriteLine($"- salarioDiario: {salarioDiario}");
                System.Diagnostics.Debug.WriteLine($"- periocidadPago: {periocidadPago}");
                System.Diagnostics.Debug.WriteLine($"- idTipoMoneda: {idTipoMoneda}");
                System.Diagnostics.Debug.WriteLine($"- cuentaIBAN: {cuentaIBAN}");
                System.Diagnostics.Debug.WriteLine($"- idBanco: {idBanco}");

                using (var contexto = new Contexto())
                {
                    var empleado = contexto.Empleados.FirstOrDefault(e => e.idEmpleado == idEmpleado);
                    if (empleado == null) 
                    {
                        System.Diagnostics.Debug.WriteLine("ERROR: Empleado no encontrado en BD");
                        return false;
                    }

                    System.Diagnostics.Debug.WriteLine($"Empleado encontrado - Valores ANTES de actualizar:");
                    System.Diagnostics.Debug.WriteLine($"- salarioAprobado: {empleado.salarioAprobado}");
                    System.Diagnostics.Debug.WriteLine($"- periocidadPago: {empleado.periocidadPago}");
                    System.Diagnostics.Debug.WriteLine($"- idTipoMoneda: {empleado.idTipoMoneda}");
                    System.Diagnostics.Debug.WriteLine($"- cuentaIBAN: {empleado.cuentaIBAN}");
                    System.Diagnostics.Debug.WriteLine($"- idBanco: {empleado.idBanco}");

                    empleado.salarioAprobado = salarioAprobado;
                    empleado.salarioDiario = salarioDiario;
                    empleado.periocidadPago = periocidadPago;
                    empleado.idTipoMoneda = idTipoMoneda;
                    empleado.cuentaIBAN = cuentaIBAN;
                    empleado.idBanco = idBanco;

                    System.Diagnostics.Debug.WriteLine($"Valores DESPUÉS de asignar:");
                    System.Diagnostics.Debug.WriteLine($"- salarioAprobado: {empleado.salarioAprobado}");
                    System.Diagnostics.Debug.WriteLine($"- periocidadPago: {empleado.periocidadPago}");
                    System.Diagnostics.Debug.WriteLine($"- idTipoMoneda: {empleado.idTipoMoneda}");
                    System.Diagnostics.Debug.WriteLine($"- cuentaIBAN: {empleado.cuentaIBAN}");
                    System.Diagnostics.Debug.WriteLine($"- idBanco: {empleado.idBanco}");

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

                    System.Diagnostics.Debug.WriteLine($"Guardando cambios en la base de datos...");
                    int registrosAfectados = contexto.SaveChanges();
                    System.Diagnostics.Debug.WriteLine($"Registros afectados: {registrosAfectados}");
                    System.Diagnostics.Debug.WriteLine($"=== ACTUALIZACIÓN COMPLETADA EXITOSAMENTE ===");
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPCIÓN en ActualizarDatosFinancieros (AccesoADatos): {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
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
                return contexto.Bancos
                    .Select(b => new BancoDto
                    {
                        idBanco = b.idBanco,
                        nombreBanco = b.nombreBanco
                    }).ToList();
            }
        }

        /// <summary>
        /// Busca un cantón por nombre y provincia, si no existe lo crea
        /// </summary>
        private int ObtenerOCrearCantonPorNombre(string nombreCanton, int idProvincia)
        {
            try
            {
                using (var contexto = new Contexto())
                {
                    // Buscar cantón existente por nombre (insensible a mayúsculas/minúsculas)
                    var canton = contexto.Canton
                        .FirstOrDefault(c => c.nombreCanton.ToLower() == nombreCanton.ToLower() && c.idProvincia == idProvincia);

                    if (canton != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"✅ Cantón encontrado: {canton.nombreCanton} (ID: {canton.idCanton})");
                        return canton.idCanton;
                    }

                    // Si no existe, crear nuevo cantón con ID único
                    var maxId = contexto.Canton.Max(c => (int?)c.idCanton) ?? 100;
                    var nuevoId = maxId + 1;

                    var nuevoCanton = new Emplaniapp.Abstracciones.ModelosAD.Canton
                    {
                        idCanton = nuevoId,
                        nombreCanton = nombreCanton.Trim(),
                        idProvincia = idProvincia
                    };

                    contexto.Canton.Add(nuevoCanton);
                    contexto.SaveChanges();

                    System.Diagnostics.Debug.WriteLine($"✅ Nuevo cantón creado: {nuevoCanton.nombreCanton} (ID: {nuevoCanton.idCanton})");
                    return nuevoCanton.idCanton;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en ObtenerOCrearCantonPorNombre: {ex.Message}");
                // En caso de error, devolver San José como fallback
                return 101;
            }
        }

        /// <summary>
        /// Busca un distrito por nombre, si no existe lo crea
        /// </summary>
        private int ObtenerOCrearDistritoPorNombre(string nombreDistrito, string nombreCanton, int idProvincia)
        {
            try
            {
                using (var contexto = new Contexto())
                {
                    // Primero obtener el ID del cantón
                    var idCanton = ObtenerOCrearCantonPorNombre(nombreCanton, idProvincia);

                    // Buscar distrito existente por nombre y cantón
                    var distrito = contexto.Distrito
                        .FirstOrDefault(d => d.nombreDistrito.ToLower() == nombreDistrito.ToLower() && d.idCanton == idCanton);

                    if (distrito != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"✅ Distrito encontrado: {distrito.nombreDistrito} (ID: {distrito.idDistrito})");
                        return distrito.idDistrito;
                    }

                    // Si no existe, crear nuevo distrito con ID único
                    var maxId = contexto.Distrito.Max(d => (int?)d.idDistrito) ?? 100;
                    var nuevoId = maxId + 1;

                    var nuevoDistrito = new Emplaniapp.Abstracciones.ModelosAD.Distrito
                    {
                        idDistrito = nuevoId,
                        nombreDistrito = nombreDistrito.Trim(),
                        idCanton = idCanton
                    };

                    contexto.Distrito.Add(nuevoDistrito);
                    contexto.SaveChanges();

                    System.Diagnostics.Debug.WriteLine($"✅ Nuevo distrito creado: {nuevoDistrito.nombreDistrito} (ID: {nuevoDistrito.idDistrito})");
                    return nuevoDistrito.idDistrito;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en ObtenerOCrearDistritoPorNombre: {ex.Message}");
                // En caso de error, devolver Carmen como fallback
                return 1;
            }
        }
    }
} 