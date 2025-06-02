// IdentityModels.cs
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Emplaniapp.UI.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Puedes añadir propiedades extra aquí, por ejemplo:
        // public string NombreCompleto { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
