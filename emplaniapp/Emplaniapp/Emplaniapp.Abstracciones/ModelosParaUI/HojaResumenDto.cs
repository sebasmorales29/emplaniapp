using System;
using System.ComponentModel;

namespace Emplaniapp.Abstracciones.ModelosParaUI
{
    public class HojaResumenDto
    {
        [DisplayName("Id Empleado")]
        public int IdEmpleado { get; set; }
        [DisplayName("Empleado")]
        public string NombreEmpleado { get; set; } = string.Empty;

        // Datos Financieros
        [DisplayName("Salario Aprobado")]
        public decimal SalarioAprobado { get; set; }
        [DisplayName("Remuneraciones")]
        public decimal TotalRemuneraciones { get; set; }
        [DisplayName("Retenciones")]
        public decimal TotalRetenciones { get; set; }

        // Liquidación
        public decimal? MontoLiquidacion { get; set; }
        public DateTime? FechaLiquidacion { get; set; }

        // Salario Neto
        [DisplayName("Salario Neto")]
        public decimal SalarioNeto { get; set; }

        // Información del periodo de pago
        public int idEstado { get; set; }
        [DisplayName("Estado")]
        public string nombreEstado { get; set; }
        public bool Aprobado { get; set; }
        public int Cedula { get; set; }
        public string NombrePuesto { get; set; }

        public int IdPagoQuincenal { get; set; }
        public int? IdPeriodoPago { get; set; }
    }
}
 