using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosParaUI
{
    public class EmpleadoDto
    {
        public int idEmpleado { get; set; }

        // Información personal
        public string nombre { get; set; }
        public string segundoNombre { get; set; }
        public string primerApellido { get; set; }
        public string segundoApellido { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public int cedula { get; set; }
        public string numeroTelefonico { get; set; }
        public string correoInstitucional { get; set; }

        // Dirección
        public int idDireccion { get; set; }

        // Cargo
        public int idCargo { get; set; }
        public string nombreCargo { get; set; }

        // Fechas laborales
        public DateTime fechaContratacion { get; set; }
        public DateTime? fechaSalida { get; set; }

        // Datos salariales
        public string periocidadPago { get; set; } 
        public decimal salarioDiario { get; set; }
        public decimal salarioAprobado { get; set; }
        public decimal salarioPorMinuto { get; set; }
        public decimal salarioPoHora { get; set; }
        public decimal salarioPorHoraExtra { get; set; }

        // Moneda y banco
        public int idMoneda { get; set; }
        public string nombreMoneda { get; set; }
        public string cuentaIBAN { get; set; }

        public int idBanco { get; set; }
        public string nombreBanco { get; set; }

        // Estado del empleado
        public int idEstado { get; set; }
        public string nombreEstado { get; set; }
    }
}
