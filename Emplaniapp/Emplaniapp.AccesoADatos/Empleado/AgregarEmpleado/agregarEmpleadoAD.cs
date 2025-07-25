using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Empleado.AgregarEmpleado;
using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.AccesoADatos.Empleado.agregarEmpleado
{
    public class agregarEmpleadoAD : IAgregarEmpleadoAD
    {
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
                        salarioDiario = empleadoDto.salarioAprobado / 15; // Corregido: 15 días por quincena
                    }
                    else if (empleadoDto.periocidadPago == "Mensual")
                    {
                        salarioDiario = empleadoDto.salarioAprobado / 30; // 30 días por mes
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
                        
                        // Campos de dirección - usar valores por defecto seguros
                        idDireccion = 1, // Campo requerido por BD - usar dirección por defecto
                        idProvincia = empleadoDto.idProvincia ?? 1,
                        idCanton = empleadoDto.idCanton ?? 1,
                        idDistrito = empleadoDto.idDistrito ?? 1,
                        idCalle = empleadoDto.idCalle ?? 1,
                        direccionDetallada = empleadoDto.direccionDetallada ?? "Dirección por defecto",
                        
                        // Campos laborales
                        idCargo = empleadoDto.idCargo,
                        fechaContratacion = empleadoDto.fechaContratacion,
                        fechaSalida = empleadoDto.fechaSalida,
                        periocidadPago = empleadoDto.periocidadPago,
                        
                        // Campos salariales calculados
                        salarioDiario = salarioDiario,
                        salarioAprobado = empleadoDto.salarioAprobado,
                        salarioPorMinuto = salarioPorMinuto,
                        salarioPoHora = salarioPorHora,
                        salarioPorHoraExtra = salarioPorHoraExtra,
                        
                        // Campos bancarios
                        idTipoMoneda = empleadoDto.idMoneda ?? 1,
                        cuentaIBAN = empleadoDto.cuentaIBAN,
                        idBanco = empleadoDto.idBanco ?? 1,
                        
                        // Estado
                        idEstado = empleadoDto.idEstado
                    };

                    System.Diagnostics.Debug.WriteLine("=== VALORES DEL NUEVO EMPLEADO ===");
                    System.Diagnostics.Debug.WriteLine("IdNetUser: " + nuevoEmpleado.IdNetUser);
                    System.Diagnostics.Debug.WriteLine("nombre: " + nuevoEmpleado.nombre);
                    System.Diagnostics.Debug.WriteLine("cedula: " + nuevoEmpleado.cedula);
                    System.Diagnostics.Debug.WriteLine("correoInstitucional: " + nuevoEmpleado.correoInstitucional);
                    System.Diagnostics.Debug.WriteLine("idDireccion: " + nuevoEmpleado.idDireccion);
                    System.Diagnostics.Debug.WriteLine("idProvincia: " + nuevoEmpleado.idProvincia);
                    System.Diagnostics.Debug.WriteLine("idCanton: " + nuevoEmpleado.idCanton);
                    System.Diagnostics.Debug.WriteLine("idDistrito: " + nuevoEmpleado.idDistrito);
                    System.Diagnostics.Debug.WriteLine("idCalle: " + nuevoEmpleado.idCalle);
                    System.Diagnostics.Debug.WriteLine("direccionDetallada: " + nuevoEmpleado.direccionDetallada);
                    System.Diagnostics.Debug.WriteLine("idCargo: " + nuevoEmpleado.idCargo);
                    System.Diagnostics.Debug.WriteLine("periocidadPago: " + nuevoEmpleado.periocidadPago);
                    System.Diagnostics.Debug.WriteLine("salarioAprobado: " + nuevoEmpleado.salarioAprobado);
                    System.Diagnostics.Debug.WriteLine("salarioDiario: " + nuevoEmpleado.salarioDiario);
                    System.Diagnostics.Debug.WriteLine("idTipoMoneda: " + nuevoEmpleado.idTipoMoneda);
                    System.Diagnostics.Debug.WriteLine("idBanco: " + nuevoEmpleado.idBanco);
                    System.Diagnostics.Debug.WriteLine("idEstado: " + nuevoEmpleado.idEstado);
                    System.Diagnostics.Debug.WriteLine("fechaContratacion: " + nuevoEmpleado.fechaContratacion);
                    System.Diagnostics.Debug.WriteLine("fechaNacimiento: " + nuevoEmpleado.fechaNacimiento);
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
                System.Diagnostics.Debug.WriteLine("❌ ERROR CRÍTICO en CrearEmpleado AccesoADatos");
                System.Diagnostics.Debug.WriteLine("Tipo de excepción: " + ex.GetType().Name);
                System.Diagnostics.Debug.WriteLine("Mensaje principal: " + ex.Message);
                
                // Identificar tipos específicos de errores
                if (ex.Message.Contains("FOREIGN KEY constraint"))
                {
                    System.Diagnostics.Debug.WriteLine("🔑 ERROR DE CLAVE FORÁNEA - Verificar que existan registros en tablas relacionadas");
                }
                else if (ex.Message.Contains("PRIMARY KEY constraint") || ex.Message.Contains("UNIQUE constraint"))
                {
                    System.Diagnostics.Debug.WriteLine("🚨 ERROR DE DUPLICACIÓN - Cédula o email ya existe");
                }
                else if (ex.Message.Contains("cannot be null"))
                {
                    System.Diagnostics.Debug.WriteLine("⚠️ ERROR DE CAMPO REQUERIDO - Campo obligatorio está nulo");
                }
                
                System.Diagnostics.Debug.WriteLine("Stack trace: " + ex.StackTrace);

                // Recursivamente mostrar todas las excepciones internas
                Exception innerEx = ex.InnerException;
                int nivel = 1;
                while (innerEx != null)
                {
                    System.Diagnostics.Debug.WriteLine($"🔍 Inner exception nivel {nivel}: " + innerEx.Message);
                    System.Diagnostics.Debug.WriteLine($"📝 Inner exception tipo {nivel}: " + innerEx.GetType().Name);
                    
                    // Información específica para errores de SQL
                    if (innerEx.GetType().Name.Contains("SqlException"))
                    {
                        System.Diagnostics.Debug.WriteLine($"🗄️ ERROR SQL ESPECÍFICO: " + innerEx.Message);
                    }
                    
                    if (innerEx.InnerException == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"🎯 Excepción más interna (raíz): " + innerEx.ToString());
                    }
                    innerEx = innerEx.InnerException;
                    nivel++;
                }
                
                System.Diagnostics.Debug.WriteLine("❌ ERROR: Retornando false - creación de empleado falló");
                return false;
            }
        }
    }
}
