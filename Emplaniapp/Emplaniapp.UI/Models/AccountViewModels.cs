using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Emplaniapp.UI.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "¿Recordar sesión?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [System.ComponentModel.DataAnnotations.Compare(
        "Password",
        ErrorMessage = "La contraseña y la confirmación no coinciden."
    )]
        public string ConfirmPassword { get; set; }
  

        // Nuevo: rol seleccionado
        [Required]
        [Display(Name = "Rol")]
        public string Role { get; set; }

        [Display(Name = "Cédula (solo para rol Empleado)")]
        public int? Cedula { get; set; }

        // Nueva: lista de roles para el dropdown
        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}
