using Emplaniapp.Abstracciones.InterfacesAD;
using Emplaniapp.Abstracciones.InterfacesParaUI;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.LogicaDeNegocio
{
    public class DatosPersonalesLN : IDatosPersonalesLN
    {
        private IDatosPersonalesAD _datosPersonalesAD;

        public DatosPersonalesLN()
        {
            _datosPersonalesAD = new DatosPersonalesAD();
        }

        public EmpleadoDto ObtenerEmpleadoPorId(int idEmpleado)
        {
            return _datosPersonalesAD.ObtenerEmpleadoPorId(idEmpleado);
        }

        public bool CrearEmpleado(EmpleadoDto empleado)
        {
            // Validaciones de negocio para crear empleado
            if (string.IsNullOrEmpty(empleado.nombre))
            {
                System.Diagnostics.Debug.WriteLine("Error: Nombre vacío");
                return false; // Faltan datos requeridos
            }
            
            if (string.IsNullOrEmpty(empleado.primerApellido))
            {
                System.Diagnostics.Debug.WriteLine("Error: Primer apellido vacío");
                return false;
            }
            
            if (string.IsNullOrEmpty(empleado.segundoApellido))
            {
                System.Diagnostics.Debug.WriteLine("Error: Segundo apellido vacío");
                return false;
            }
            
            if (empleado.cedula <= 0)
            {
                System.Diagnostics.Debug.WriteLine("Error: Cédula inválida: " + empleado.cedula);
                return false;
            }
            
            if (string.IsNullOrEmpty(empleado.numeroTelefonico))
            {
                System.Diagnostics.Debug.WriteLine("Error: Teléfono vacío");
                return false;
            }
            
            if (string.IsNullOrEmpty(empleado.correoInstitucional))
            {
                System.Diagnostics.Debug.WriteLine("Error: Correo vacío");
                return false;
            }
            
            if (empleado.idCargo <= 0)
            {
                System.Diagnostics.Debug.WriteLine("Error: Cargo inválido: " + empleado.idCargo);
                return false;
            }
            
            if (empleado.salarioAprobado <= 0)
            {
                System.Diagnostics.Debug.WriteLine("Error: Salario inválido: " + empleado.salarioAprobado);
                return false;
            }
            
            if (string.IsNullOrEmpty(empleado.periocidadPago))
            {
                System.Diagnostics.Debug.WriteLine("Error: Periodicidad vacía");
                return false;
            }
            
            if (empleado.idMoneda <= 0)
            {
                System.Diagnostics.Debug.WriteLine("Error: Moneda inválida: " + empleado.idMoneda);
                return false;
            }
            
            if (empleado.idBanco <= 0)
            {
                System.Diagnostics.Debug.WriteLine("Error: Banco inválido: " + empleado.idBanco);
                return false;
            }
            
            if (string.IsNullOrEmpty(empleado.cuentaIBAN))
            {
                System.Diagnostics.Debug.WriteLine("Error: IBAN vacío");
                return false;
            }

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

            // Validar cédula (debe tener 9 dígitos)
            if (empleado.cedula < 100000000 || empleado.cedula > 999999999)
            {
                System.Diagnostics.Debug.WriteLine("Error: Cédula no tiene 9 dígitos: " + empleado.cedula);
                return false; // Cédula inválida
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
                bool resultado = _datosPersonalesAD.CrearEmpleado(empleado);
                System.Diagnostics.Debug.WriteLine("Resultado de AccesoADatos: " + resultado);
                return resultado;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Excepción en CrearEmpleado: " + ex.Message);
                return false;
            }
        }

        public bool ActualizarDatosPersonales(EmpleadoDto empleado)
        {
            // Aquí podríamos agregar validaciones de negocio
            if (string.IsNullOrEmpty(empleado.nombre) || 
                string.IsNullOrEmpty(empleado.primerApellido) ||
                empleado.cedula <= 0)
            {
                return false;
            }

            return _datosPersonalesAD.ActualizarDatosPersonales(empleado);
        }

        public bool ActualizarDatosLaborales(int idEmpleado, int idCargo, DateTime fechaContratacion, DateTime? fechaSalida)
        {
            // Validaciones de negocio
            if (fechaSalida.HasValue && fechaSalida.Value <= fechaContratacion)
            {
                return false; // La fecha de salida no puede ser anterior a la de contratación
            }

            return _datosPersonalesAD.ActualizarDatosLaborales(idEmpleado, idCargo, fechaContratacion, fechaSalida);
        }

        public bool ActualizarDatosFinancieros(int idEmpleado, decimal salarioAprobado, decimal salarioDiario, 
            string periocidadPago, int idTipoMoneda, string cuentaIBAN, int idBanco)
        {
            // Validaciones de negocio
            if (salarioAprobado <= 0 || string.IsNullOrEmpty(cuentaIBAN))
            {
                return false;
            }

            return _datosPersonalesAD.ActualizarDatosFinancieros(idEmpleado, salarioAprobado, salarioDiario, 
                periocidadPago, idTipoMoneda, cuentaIBAN, idBanco);
        }

        public List<CargoDto> ObtenerCargos()
        {
            return _datosPersonalesAD.ObtenerCargos();
        }

        public List<MonedaDto> ObtenerTiposMoneda()
        {
            return _datosPersonalesAD.ObtenerTiposMoneda();
        }

        public List<BancoDto> ObtenerBancos()
        {
            return _datosPersonalesAD.ObtenerBancos();
        }
    }
} 