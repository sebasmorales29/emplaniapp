using System.ComponentModel.DataAnnotations;

namespace Emplaniapp.UI.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }
    }
}
