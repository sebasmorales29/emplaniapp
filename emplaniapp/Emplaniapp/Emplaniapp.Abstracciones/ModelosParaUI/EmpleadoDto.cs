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
        [Required(ErrorMessage = "La fecha de nacimiento del empleado es obligatoria.")]
        public DateTime fechaNacimiento { get; set; }

        [DisplayName("Cédula")]
        [Required(ErrorMessage = "La cédula del empleado es obligatoria.")]
        [Range(100000000, 999999999, ErrorMessage = "La cédula debe tener 9 dígitos")]
        public int cedula { get; set; }

        [DisplayName("Teléfono")]
        [Required(ErrorMessage = "El teléfono del empleado es obligatorio.")]
        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        public string numeroTelefonico { get; set; }

        [DisplayName("Correo Institucional")]
        [Required(ErrorMessage = "El correo institucional del empleado es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        public string correoInstitucional { get; set; }

        [DisplayName("Nombre Completo")]
        public string nombreCompleto { get; set; }

        // Dirección
        [DisplayName("Provincia")]
        [Required(ErrorMessage = "Por favor seleccionar una provincia.")]
        public int? idProvincia { get; set; }
        
        [DisplayName("Provincia")]
        public string nombreProvincia { get; set; }
        
        [DisplayName("Cantón")]
        public string nombreCanton { get; set; }
        
        [DisplayName("Distrito")]
        public string nombreDistrito { get; set; }

        [DisplayName("Dirección Detallada")]
        //[Required(ErrorMessage = "La dirección detallada del empleado es obligatorio.")]
        public string direccionDetallada { get; set; }

        [DisplayName("Dirección")]
        public string direccionCompleta { get; set; }

        // Cargo
        [DisplayName("N° de Ocupación")]
        [Required(ErrorMessage = "El cargo del empleado es obligatorio.")]
        public int? idCargo { get; set; }

        [DisplayName("Cargo")]
        public string nombreCargo { get; set; }

        // Fechas laborales
        [DisplayName("Fecha de Contratación")]
        [Required(ErrorMessage = "La fecha de contratación del empleado es obligatoria.")]
        public DateTime fechaContratacion { get; set; }

        [DisplayName("Fecha Salida")]
        public DateTime? fechaSalida { get; set; }

        // Datos salariales
        [DisplayName("Periodicidad de Pago")]
        [Required(ErrorMessage = "La periodicidad de pago del empleado es obligatoria.")]
        public string periocidadPago { get; set; }

        [DisplayName("Salario Diario")]
        public decimal salarioDiario { get; set; }

        [DisplayName("Salario Aprobado")]
        [Required(ErrorMessage = "El salario aprobado del empleado es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El salario aprobado debe ser un valor positivo")]
        public decimal salarioAprobado { get; set; }

        [DisplayName("Salario Por Minuto")]
        public decimal salarioPorMinuto { get; set; }

        [DisplayName("Salario Por Hora")]
        public decimal salarioPoHora { get; set; }

        [DisplayName("Salario Por Hora Extra")]
        public decimal salarioPorHoraExtra { get; set; }

        // Moneda y banco
        [DisplayName("Moneda")]
        [Required(ErrorMessage = "La moneda del empleado es obligatoria.")]
        public int? idMoneda { get; set; }

        [DisplayName("Moneda")]
        public string nombreMoneda { get; set; }

        [DisplayName("Cuenta IBAN")]
        [Required(ErrorMessage = "La cuenta IBAN del empleado es obligatoria.")]
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
        [Required(ErrorMessage = "Por favor, crear un usuario")]
        public string UserName { get; set; }

        [DisplayName("Contraseña")]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres.", MinimumLength = 6)]
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }

        [DisplayName("Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación de contraseña no coinciden.")]
        public string ConfirmPassword { get; set; }

        [DisplayName("Roles")]
        public string RolesUsuario { get; set; }
    }
}
