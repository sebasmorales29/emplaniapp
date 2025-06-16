using System;
using System.ComponentModel.DataAnnotations;

namespace Emplaniapp.UI.Models
{
    public class AgregarEmpleadoViewModel
    {
        // DATOS PERSONALES
        [Required(ErrorMessage = "El nombre es requerido")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Segundo Nombre")]
        public string SegundoNombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es requerido")]
        [Display(Name = "Primer Apellido")]
        public string PrimerApellido { get; set; }

        [Required(ErrorMessage = "El segundo apellido es requerido")]
        [Display(Name = "Segundo Apellido")]
        public string SegundoApellido { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "La cédula es requerida")]
        [Display(Name = "Cédula")]
        [Range(100000000, 999999999, ErrorMessage = "La cédula debe tener 9 dígitos")]
        public int Cedula { get; set; }

        [Required(ErrorMessage = "El número telefónico es requerido")]
        [Display(Name = "Número Telefónico")]
        public string NumeroTelefonico { get; set; }

        [Required(ErrorMessage = "El correo institucional es requerido")]
        [Display(Name = "Correo Institucional")]
        [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido")]
        public string CorreoInstitucional { get; set; }

        // DATOS LABORALES
        [Required(ErrorMessage = "El cargo es requerido")]
        [Display(Name = "Cargo")]
        public int IdCargo { get; set; }

        [Required(ErrorMessage = "La fecha de contratación es requerida")]
        [Display(Name = "Fecha de Contratación")]
        [DataType(DataType.Date)]
        public DateTime FechaContratacion { get; set; }

        [Display(Name = "Fecha de Salida")]
        [DataType(DataType.Date)]
        public DateTime? FechaSalida { get; set; }

        [Required(ErrorMessage = "La periodicidad de pago es requerida")]
        [Display(Name = "Periodicidad de Pago")]
        public string PeriocidadPago { get; set; }

        [Required(ErrorMessage = "El salario aprobado es requerido")]
        [Display(Name = "Salario Aprobado")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El salario debe ser mayor a cero")]
        public decimal SalarioAprobado { get; set; }

        // DATOS BANCARIOS
        [Required(ErrorMessage = "El tipo de moneda es requerido")]
        [Display(Name = "Tipo de Moneda")]
        public int IdTipoMoneda { get; set; }

        [Required(ErrorMessage = "La cuenta IBAN es requerida")]
        [Display(Name = "Cuenta IBAN")]
        public string CuentaIBAN { get; set; }

        [Required(ErrorMessage = "El banco es requerido")]
        [Display(Name = "Banco")]
        public int IdBanco { get; set; }

        // DATOS AUTOMÁTICOS (se calculan)
        public decimal SalarioDiario { get; set; }
        public decimal SalarioPorMinuto { get; set; }
        public decimal SalarioPorHora { get; set; }
        public decimal SalarioPorHoraExtra { get; set; }

        // DIRECCIÓN (por ahora usaremos una dirección por defecto)
        public int IdDireccion { get; set; } = 1;

        // ESTADO (por defecto Activo)
        public int IdEstado { get; set; } = 1;

        // --- Campos para la creación del usuario ---

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [Display(Name = "Nombre de Usuario")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio")]
        [Display(Name = "Rol de Usuario")]
        public string Role { get; set; }
    }
} 