using Emplaniapp.Abstracciones.InterfacesAD.Empleado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Emplaniapp.AccesoADatos.Empleado
{
    public class EmpleadoAD : IEmpleadoAD
    {
        private Contexto contexto;

        public EmpleadoAD()
        {
            contexto = new Contexto();
        }

        public List<Abstracciones.ModelosAD.Empleado> ListarTodos()
        {
            try
            {
                return contexto.Empleados.Where(e => e.idEstado != 3).ToList(); // Excluye eliminados
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar empleados: " + ex.Message);
            }
        }

        public Abstracciones.ModelosAD.Empleado ObtenerPorId(int id)
        {
            try
            {
                return contexto.Empleados.FirstOrDefault(e => e.Id == id && e.idEstado != 3);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener empleado: " + ex.Message);
            }
        }

        public bool Insertar(Abstracciones.ModelosAD.Empleado empleado)
        {
            try
            {
                // Validaciones básicas antes de insertar
                if (empleado == null)
                    throw new ArgumentNullException("empleado", "El empleado no puede ser nulo");

                if (string.IsNullOrEmpty(empleado.nombre))
                    throw new ArgumentException("El nombre es requerido");

                if (string.IsNullOrEmpty(empleado.primerApellido))
                    throw new ArgumentException("El primer apellido es requerido");

                if (string.IsNullOrEmpty(empleado.segundoApellido))
                    throw new ArgumentException("El segundo apellido es requerido");

                if (empleado.cedula <= 0)
                    throw new ArgumentException("La cédula debe ser válida");

                // Validaciones de foreign keys - solo si los valores son diferentes de los defaults
                if (empleado.idCargo <= 0)
                {
                    // Si no hay cargo seleccionado, usar un valor por defecto temporal
                    empleado.idCargo = 1;
                }

                if (empleado.idTipoMoneda <= 0)
                {
                    // Si no hay moneda seleccionada, usar un valor por defecto temporal
                    empleado.idTipoMoneda = 1;
                }

                if (empleado.idBanco <= 0)
                {
                    // Si no hay banco seleccionado, usar un valor por defecto temporal
                    empleado.idBanco = 1;
                }

                if (empleado.idEstado <= 0)
                {
                    // Si no hay estado seleccionado, usar activo por defecto
                    empleado.idEstado = 1;
                }

                if (empleado.idDireccion <= 0)
                {
                    // Si no hay dirección seleccionada, usar un valor por defecto temporal
                    empleado.idDireccion = 1;
                }

                // Verificar que la cédula no exista ya
                var existeCedula = contexto.Empleados.Any(e => e.cedula == empleado.cedula && e.idEstado != 3);
                if (existeCedula)
                    throw new ArgumentException($"Ya existe un empleado con la cédula {empleado.cedula}");

                // Verificar que el correo no exista ya (solo si no está vacío)
                if (!string.IsNullOrEmpty(empleado.correoInstitucional))
                {
                    var existeCorreo = contexto.Empleados.Any(e => e.correoInstitucional == empleado.correoInstitucional && e.idEstado != 3);
                    if (existeCorreo)
                        throw new ArgumentException($"Ya existe un empleado con el correo {empleado.correoInstitucional}");
                }

                // Generar el próximo ID manualmente ya que la BD no tiene auto-incremento
                var maxId = contexto.Empleados.Any() ? contexto.Empleados.Max(e => e.Id) : 0;
                empleado.Id = maxId + 1;

                // Log para depuración
                System.Diagnostics.Debug.WriteLine($"=== INSERTAR EMPLEADO - ID ASIGNADO ===");
                System.Diagnostics.Debug.WriteLine($"Max ID encontrado: {maxId}");
                System.Diagnostics.Debug.WriteLine($"Nuevo ID asignado: {empleado.Id}");
                System.Diagnostics.Debug.WriteLine($"nombre: {empleado.nombre}");
                System.Diagnostics.Debug.WriteLine($"=========================================");

                contexto.Empleados.Add(empleado);
                int resultado = contexto.SaveChanges();
                return resultado > 0;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                var errorMessages = dbEx.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat("Error de validación de Entity Framework: ", fullErrorMessage);
                throw new Exception(exceptionMessage);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException dbUpdateEx)
            {
                var innerException = dbUpdateEx.InnerException?.InnerException?.Message ?? dbUpdateEx.InnerException?.Message ?? dbUpdateEx.Message;
                throw new Exception($"Error al actualizar la base de datos: {innerException}");
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Error al insertar empleado: {innerException}");
            }
        }

        public bool Actualizar(Abstracciones.ModelosAD.Empleado empleado)
        {
            try
            {
                var empleadoExistente = contexto.Empleados.FirstOrDefault(e => e.Id == empleado.Id);
                if (empleadoExistente != null)
                {
                    // Actualizar propiedades
                    empleadoExistente.nombre = empleado.nombre;
                    empleadoExistente.segundoNombre = empleado.segundoNombre;
                    empleadoExistente.primerApellido = empleado.primerApellido;
                    empleadoExistente.segundoApellido = empleado.segundoApellido;
                    empleadoExistente.fechaNacimiento = empleado.fechaNacimiento;
                    empleadoExistente.cedula = empleado.cedula;
                    empleadoExistente.numeroTelefonico = empleado.numeroTelefonico;
                    empleadoExistente.correoInstitucional = empleado.correoInstitucional;
                    empleadoExistente.idDireccion = empleado.idDireccion;
                    empleadoExistente.idCargo = empleado.idCargo;
                    empleadoExistente.fechaContratacion = empleado.fechaContratacion;
                    empleadoExistente.fechaSalida = empleado.fechaSalida;
                    empleadoExistente.periocidadPago = empleado.periocidadPago;
                    empleadoExistente.salarioDiario = empleado.salarioDiario;
                    empleadoExistente.salarioAprobado = empleado.salarioAprobado;
                    empleadoExistente.salarioPorMinuto = empleado.salarioPorMinuto;
                    empleadoExistente.salarioPoHora = empleado.salarioPoHora;
                    empleadoExistente.salarioPorHoraExtra = empleado.salarioPorHoraExtra;
                    empleadoExistente.idTipoMoneda = empleado.idTipoMoneda;
                    empleadoExistente.cuentaIBAN = empleado.cuentaIBAN;
                    empleadoExistente.idBanco = empleado.idBanco;
                    empleadoExistente.idEstado = empleado.idEstado;

                    int resultado = contexto.SaveChanges();
                    return resultado > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar empleado: " + ex.Message);
            }
        }

        public bool Eliminar(int id)
        {
            try
            {
                var empleado = contexto.Empleados.FirstOrDefault(e => e.Id == id);
                if (empleado != null)
                {
                    // Eliminación lógica - cambiar estado a eliminado (asumiendo 3 = eliminado)
                    empleado.idEstado = 3;
                    int resultado = contexto.SaveChanges();
                    return resultado > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar empleado: " + ex.Message);
            }
        }

        public bool ActivarDesactivar(int id, int nuevoEstado)
        {
            try
            {
                var empleado = contexto.Empleados.FirstOrDefault(e => e.Id == id);
                if (empleado != null)
                {
                    empleado.idEstado = nuevoEstado;
                    int resultado = contexto.SaveChanges();
                    return resultado > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al activar/desactivar empleado: " + ex.Message);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                contexto?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
} 