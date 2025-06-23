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
            // Validar que la fecha de nacimiento sea válida (mayor de edad)
            if (empleado.fechaNacimiento > DateTime.Now.AddYears(-18))
            {
                System.Diagnostics.Debug.WriteLine("Error: Menor de edad: " + empleado.fechaNacimiento);
                return false; // Debe ser mayor de edad
            }

            // Validar que la fecha de contratación no sea futura
            if (empleado.fechaContratacion > DateTime.Now)
            {
                System.Diagnostics.Debug.WriteLine("Error: Fecha contratación futura: " + empleado.fechaContratacion);
                return false; // La fecha de contratación no puede ser futura
            }
            // Validar periodicidad de pago
            if (empleado.periocidadPago != "Quincenal" && empleado.periocidadPago != "Mensual")
            {
                System.Diagnostics.Debug.WriteLine("Error: Periodicidad inválida: " + empleado.periocidadPago);
                return false; // Periodicidad inválida
            }

            System.Diagnostics.Debug.WriteLine("Todas las validaciones pasaron, llamando a AccesoADatos");

            try
            {
                bool resultado = _agregarEmpleadoAD.CrearEmpleado(empleado);
                System.Diagnostics.Debug.WriteLine("Resultado de AccesoADatos: " + resultado);
                return resultado;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Excepción en CrearEmpleado: " + ex.Message);
                return false;
            }
        }
    }
}
