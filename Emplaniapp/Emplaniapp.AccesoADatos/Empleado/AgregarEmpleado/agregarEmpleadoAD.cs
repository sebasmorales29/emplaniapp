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
                        salarioDiario = empleadoDto.salarioAprobado / 30;
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
    }
}
