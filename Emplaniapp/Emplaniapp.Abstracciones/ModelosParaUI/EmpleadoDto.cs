using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Emplaniapp.Abstracciones.ModelosParaUI
{
    public class EmpleadoDto
    {
        // (El campo primerPrimerApellido parece sobrante; puedes eliminarlo si no lo usas)
        public object primerPrimerApellido;

        [DisplayName("Id empleado")]
        public int idEmpleado { get; set; }

        // Información personal
        [DisplayName("Primer Nombre")]
        [Required(ErrorMessage = "El primer nombre del empleado es obligatorio.")]
        public string nombre { get; set; }

        [DisplayName("Segundo Nombre")]
        public string segundoNombre { get; set; }

        [DisplayName("Primer Apellido")]
        [Required(ErrorMessage = "El primer apellido del empleado es obligatorio.")]
        public string primerApellido { get; set; }

        [DisplayName("Segundo Apellido")]
        [Required(ErrorMessage = "El segundo apellido del empleado es obligatorio.")]
        public string segundoApellido { get; set; }

        [DisplayName("Fecha de Nacimiento")]
        [Required(ErrorMessage = "La fecha de nacimiento del empleado es obligatorio.")]
        public DateTime fechaNacimiento { get; set; }

        [DisplayName("Identificación")]
        [Required(ErrorMessage = "La identificación del empleado es obligatorio.")]
        [Range(100000000, 999999999, ErrorMessage = "La cédula debe tener 9 dígitos")]
        public int cedula { get; set; }

        [DisplayName("Teléfono")]
        [Required(ErrorMessage = "El teléfono del empleado es obligatorio.")]
        [Phone(ErrorMessage = "No cumple con el formato del teléfono")]
        public string numeroTelefonico { get; set; }

        [DisplayName("Correo institucional")]
        [Required(ErrorMessage = "El correo del empleado es obligatorio.")]
        [EmailAddress(ErrorMessage = "No cumple con el formato del correo")]
        public string correoInstitucional { get; set; }

        [DisplayName("Nombre Completo")]
        public string nombreCompleto { get; set; }

        // Dirección
        public int? idDireccion { get; set; }

        [DisplayName("Dirección")]
        public string direccionCompleta { get; set; }

        // Cargo
        [DisplayName("Cargo")]
        [Required(ErrorMessage = "El cargo del empleado es obligatorio.")]
        public int? idCargo { get; set; }

        [DisplayName("Cargo")]
        public string nombreCargo { get; set; }

        // Fechas laborales
        [DisplayName("Fecha Contratación")]
        [Required(ErrorMessage = "La fecha de contratación del empleado es obligatorio.")]
        public DateTime fechaContratacion { get; set; }

        [DisplayName("Fecha Salida")]
        public DateTime? fechaSalida { get; set; }

        // Datos salariales
        [DisplayName("Periodicidad Pago")]
        [Required(ErrorMessage = "La periodicidad de pago del empleado es obligatorio.")]
        public string periocidadPago { get; set; }

        [DisplayName("Salario Diario")]
        public decimal salarioDiario { get; set; }

        [DisplayName("Salario Aprobado")]
        [Required(ErrorMessage = "El salario aprobado del empleado es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El salario aprobado debe ser un valor positivo.")]
        public decimal salarioAprobado { get; set; }

        [DisplayName("Salario Por Minuto")]
        public decimal salarioPorMinuto { get; set; }

        [DisplayName("Salario Por Hora")]
        public decimal salarioPoHora { get; set; }

        [DisplayName("Salario Por Hora Extra")]
        public decimal salarioPorHoraExtra { get; set; }

        // Moneda y banco
        [DisplayName("Moneda")]
        [Required(ErrorMessage = "La moneda del empleado es obligatorio.")]
        public int? idMoneda { get; set; }

        [DisplayName("Moneda")]
        public string nombreMoneda { get; set; }

        [DisplayName("Cuenta IBAN")]
        [Required(ErrorMessage = "La cuenta IBAN del empleado es obligatorio.")]
        public string cuentaIBAN { get; set; }

        [DisplayName("Banco")]
        [Required(ErrorMessage = "El banco del empleado es obligatorio.")]
        public int? idBanco { get; set; }

        [DisplayName("Banco")]
        public string nombreBanco { get; set; }

        // Estado del empleado
        public int idEstado { get; set; }

        [DisplayName("Estado")]
        public string nombreEstado { get; set; }

        public string IdNetUser { get; set; }

        [DisplayName("Usuario")]
        public string UserName { get; set; }

        [DisplayName("Contraseña")]
        public string Password { get; set; }

        [DisplayName("Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación de contraseña no coinciden.")]
        public string ConfirmPassword { get; set; }

        [DisplayName("Rol")]
        public string Role { get; set; }
    }
}
