using System.ComponentModel.DataAnnotations;

namespace Emplaniapp.UI.Models
{
    public class DatosFinancierosViewModel
    {
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "La periodicidad de pago es requerida")]
        [Display(Name = "Periodicidad de pago")]
        public string PeriocidadPago { get; set; }

        [Required(ErrorMessage = "El salario aprobado es requerido")]
        [Display(Name = "Salario Aprobado")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El salario debe ser mayor a cero")]
        public decimal SalarioAprobado { get; set; }

        [Display(Name = "Salario Diario")]
        public decimal SalarioDiario { get; set; }

        [Required(ErrorMessage = "El tipo de moneda es requerido")]
        [Display(Name = "Tipo de Moneda")]
        public int? IdTipoMoneda { get; set; }
        
        public string TipoMoneda { get; set; }

        [Required(ErrorMessage = "La cuenta IBAN es requerida")]
        [Display(Name = "Cuenta IBAN")]
        public string CuentaIBAN { get; set; }

        [Required(ErrorMessage = "El banco es requerido")]
        [Display(Name = "Banco")]
        public int? IdBanco { get; set; }
        
        public string Banco { get; set; }
    }
} 