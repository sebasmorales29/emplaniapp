using System.ComponentModel.DataAnnotations;

namespace Emplaniapp.UI.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Code { get; set; }

        [Required, StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password), Display(Name = "Nueva contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password), Display(Name = "Confirmar contraseña")]
        [Required]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}