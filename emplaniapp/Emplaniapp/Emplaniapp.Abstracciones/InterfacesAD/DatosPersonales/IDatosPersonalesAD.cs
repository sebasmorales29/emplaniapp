using Emplaniapp.Abstracciones.ModelosParaUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.InterfacesAD
{
    public interface IDatosPersonalesAD
    {
        EmpleadoDto ObtenerEmpleadoPorId(int idEmpleado);
        bool ActualizarDatosPersonales(EmpleadoDto empleado);
        bool ActualizarDatosLaborales(int idEmpleado, int idCargo, DateTime fechaContratacion, DateTime? fechaSalida);
        bool ActualizarDatosFinancieros(int idEmpleado, decimal salarioAprobado, decimal salarioDiario, 
            string periocidadPago, int idTipoMoneda, string cuentaIBAN, int idBanco);
        bool CrearEmpleado(EmpleadoDto empleado);
        List<CargoDto> ObtenerCargos();
        List<MonedaDto> ObtenerTiposMoneda();
        List<BancoDto> ObtenerBancos();
    }
} 