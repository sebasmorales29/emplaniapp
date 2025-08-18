using System;
using System.Configuration;
using Emplaniapp.Abstracciones.Entidades;
using Emplaniapp.AccesoADatos;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Emplaniapp.UI
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
            // Puedes dejar esta ctor vacía; toda la config se hace en Create(...)
        }

        public static ApplicationUserManager Create(
            IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var manager = new ApplicationUserManager(
                new UserStore<ApplicationUser>(context.Get<Contexto>()) // tu DbContext
            );

            // ===== Validación de usuario =====
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // ===== Validación de contraseña =====
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
                RequireNonLetterOrDigit = false
            };

            // ===== Lockout (opcional pero recomendado) =====
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // ===== Email Service (usa SmtpClient con Web.config) =====
            manager.EmailService = new EmailService();

            // ===== Proveedor de tokens con caducidad =====
            // Lee la vida del token desde appSettings; por defecto 24h
            int hours;
            if (!int.TryParse(ConfigurationManager.AppSettings["ResetTokenLifespanHours"], out hours))
                hours = 24;

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(
                    dataProtectionProvider.Create("ASP.NET Identity - Emplaniapp"))
                {
                    TokenLifespan = TimeSpan.FromHours(hours)
                };
            }

            return manager;
        }
    }

    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(
            ApplicationUserManager userManager,
            IAuthenticationManager authManager)
            : base(userManager, authManager)
        { }

        public static ApplicationSignInManager Create(
            IdentityFactoryOptions<ApplicationSignInManager> options,
            IOwinContext context)
        {
            return new ApplicationSignInManager(
                context.GetUserManager<ApplicationUserManager>(),
                context.Authentication
            );
        }
    }

    // --------- RoleManager (como lo tenías) ---------
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> store)
            : base(store)
        { }

        public static ApplicationRoleManager Create(
            IdentityFactoryOptions<ApplicationRoleManager> options,
            IOwinContext context)
        {
            return new ApplicationRoleManager(
                new RoleStore<IdentityRole>(context.Get<Contexto>())
            );
        }
    }
}