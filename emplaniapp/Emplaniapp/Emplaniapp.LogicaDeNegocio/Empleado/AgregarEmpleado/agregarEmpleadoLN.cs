using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Empleado.AgregarEmpleado;
using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.AgregarEmpleado;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Empleado.agregarEmpleado;

namespace Emplaniapp.LogicaDeNegocio.Empleado.AgregarEmpleado
{
    public class agregarEmpleadoLN : IAgregarEmpleadoLN
    {
        private IAgregarEmpleadoAD _agregarEmpleadoAD;
        public agregarEmpleadoLN()
        {
            _agregarEmpleadoAD = new agregarEmpleadoAD();
        }

        public bool CrearEmpleado(EmpleadoDto empleado)
        {
            System.Diagnostics.Debug.WriteLine("🔍 INICIO - Validaciones de lógica de negocio");
            System.Diagnostics.Debug.WriteLine($"Empleado: {empleado.nombre} {empleado.primerApellido}");
            System.Diagnostics.Debug.WriteLine($"Cédula: {empleado.cedula}");
            System.Diagnostics.Debug.WriteLine($"Fecha nacimiento: {empleado.fechaNacimiento}");
            System.Diagnostics.Debug.WriteLine($"Fecha contratación: {empleado.fechaContratacion}");
            System.Diagnostics.Debug.WriteLine($"Periodicidad: {empleado.periocidadPago}");
            System.Diagnostics.Debug.WriteLine($"Salario aprobado: {empleado.salarioAprobado}");
            System.Diagnostics.Debug.WriteLine($"Correo: {empleado.correoInstitucional}");
            System.Diagnostics.Debug.WriteLine($"IdNetUser: {empleado.IdNetUser}");

            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(empleado.nombre))
            {
                System.Diagnostics.Debug.WriteLine("❌ Error: Nombre es obligatorio");
                return false;
            }

            if (string.IsNullOrWhiteSpace(empleado.primerApellido))
            {
                System.Diagnostics.Debug.WriteLine("❌ Error: Primer apellido es obligatorio");
                return false;
            }

            if (empleado.cedula <= 0)
            {
                System.Diagnostics.Debug.WriteLine("❌ Error: Cédula inválida: " + empleado.cedula);
                return false;
            }

            // Validar que la cédula tenga el formato correcto (9 dígitos)
            if (empleado.cedula <= 100000000 || empleado.cedula >= 999999999)
            {
                System.Diagnostics.Debug.WriteLine("❌ Error: Cédula debe tener 9 dígitos: " + empleado.cedula);
                return false;
            }

            // Validar que la fecha de nacimiento sea válida (mayor de edad)
            if (empleado.fechaNacimiento > DateTime.Now.AddYears(-18))
            {
                System.Diagnostics.Debug.WriteLine("❌ Error: Menor de edad: " + empleado.fechaNacimiento);
                return false; // Debe ser mayor de edad
            }

            // Validar que la fecha de contratación no sea futura
            if (empleado.fechaContratacion > DateTime.Now)
            {
                System.Diagnostics.Debug.WriteLine("❌ Error: Fecha contratación futura: " + empleado.fechaContratacion);
                return false; // La fecha de contratación no puede ser futura
            }

            // Validar periodicidad de pago
            if (empleado.periocidadPago != "Quincenal" && empleado.periocidadPago != "Mensual")
            {
                System.Diagnostics.Debug.WriteLine("❌ Error: Periodicidad inválida: " + empleado.periocidadPago);
                return false; // Periodicidad inválida
            }

            // Validar salario aprobado
            if (empleado.salarioAprobado <= 0)
            {
                System.Diagnostics.Debug.WriteLine("❌ Error: Salario aprobado debe ser mayor a 0: " + empleado.salarioAprobado);
                return false;
            }

            System.Diagnostics.Debug.WriteLine("✅ Validaciones básicas pasaron, verificando duplicados...");

            // Verificar que la cédula no esté duplicada
            try
            {
                var contexto = new Emplaniapp.AccesoADatos.Contexto();
                try
                {
                    System.Diagnostics.Debug.WriteLine("🔍 Verificando cédula duplicada...");
                    var empleadoExistente = contexto.Empleados.FirstOrDefault(e => e.cedula == empleado.cedula);
                    if (empleadoExistente != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Error: La cédula {empleado.cedula} ya está registrada para el empleado {empleadoExistente.nombre} {empleadoExistente.primerApellido}");
                        return false;
                    }
                    System.Diagnostics.Debug.WriteLine($"✅ Cédula {empleado.cedula} disponible");

                    // Verificar que el correo electrónico no esté duplicado
                    System.Diagnostics.Debug.WriteLine("🔍 Verificando correo duplicado...");
                    var empleadoConEmail = contexto.Empleados.FirstOrDefault(e => e.correoInstitucional == empleado.correoInstitucional);
                    if (empleadoConEmail != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Error: El correo {empleado.correoInstitucional} ya está registrado para el empleado {empleadoConEmail.nombre} {empleadoConEmail.primerApellido}");
                        return false;
                    }
                    System.Diagnostics.Debug.WriteLine($"✅ Correo {empleado.correoInstitucional} disponible");

                    // Verificar que el IdNetUser no esté duplicado
                    System.Diagnostics.Debug.WriteLine("🔍 Verificando IdNetUser duplicado...");
                    var empleadoConIdNetUser = contexto.Empleados.FirstOrDefault(e => e.IdNetUser == empleado.IdNetUser);
                    if (empleadoConIdNetUser != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Error: El IdNetUser {empleado.IdNetUser} ya está registrado para el empleado {empleadoConIdNetUser.nombre} {empleadoConIdNetUser.primerApellido}");
                        return false;
                    }
                    System.Diagnostics.Debug.WriteLine($"✅ IdNetUser {empleado.IdNetUser} disponible");
                }
                finally
                {
                    contexto.Dispose();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error al verificar datos duplicados: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"📚 Stack trace: {ex.StackTrace}");
                return false;
            }

            System.Diagnostics.Debug.WriteLine("✅ Todas las validaciones pasaron, llamando a AccesoADatos");

            try
            {
                bool resultado = _agregarEmpleadoAD.CrearEmpleado(empleado);
                System.Diagnostics.Debug.WriteLine($"📊 Resultado de AccesoADatos: {resultado}");
                
                // Si se creó exitosamente, registrar en el historial
                if (resultado)
                {
                    try
                    {
                        var historialLN = new Emplaniapp.LogicaDeNegocio.Historial.RegistrarEventoHistorialLN();
                        var idEmpleadoCreado = ObtenerIdEmpleadoCreado(empleado.cedula);
                        
                        if (idEmpleadoCreado > 0)
                        {
                            historialLN.RegistrarCreacionEmpleado(idEmpleadoCreado, empleado.IdNetUser);
                            System.Diagnostics.Debug.WriteLine($"✅ Evento de creación registrado en historial para empleado {idEmpleadoCreado}");
                        }
                    }
                    catch (Exception exHistorial)
                    {
                        System.Diagnostics.Debug.WriteLine($"⚠️ Advertencia: No se pudo registrar en historial: {exHistorial.Message}");
                        // No fallar la creación por error en historial
                    }
                }
                
                return resultado;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Excepción en CrearEmpleado LN: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"📚 Stack trace: {ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// Obtiene el ID del empleado recién creado basándose en la cédula
        /// </summary>
        private int ObtenerIdEmpleadoCreado(int cedula)
        {
            try
            {
                using (var contexto = new Emplaniapp.AccesoADatos.Contexto())
                {
                    var empleado = contexto.Empleados
                        .Where(e => e.cedula == cedula)
                        .OrderByDescending(e => e.idEmpleado)
                        .FirstOrDefault();
                    
                    return empleado?.idEmpleado ?? 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error al obtener ID del empleado creado: {ex.Message}");
                return 0;
            }
        }
    }
}
