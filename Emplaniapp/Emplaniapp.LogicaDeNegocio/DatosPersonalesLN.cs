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