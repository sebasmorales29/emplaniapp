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
            if (empleado.cedula < 100000000 || empleado.cedula > 999999999)
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

            System.Diagnostics.Debug.WriteLine("✅ Todas las validaciones pasaron, llamando a AccesoADatos");

            try
            {
                bool resultado = _agregarEmpleadoAD.CrearEmpleado(empleado);
                System.Diagnostics.Debug.WriteLine($"📊 Resultado de AccesoADatos: {resultado}");
                return resultado;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Excepción en CrearEmpleado LN: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"📚 Stack trace: {ex.StackTrace}");
                return false;
            }
        }
    }
}
