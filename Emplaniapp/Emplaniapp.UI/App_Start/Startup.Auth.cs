using System;
using System.Threading.Tasks;
using System.Security.Claims;
using Emplaniapp.Abstracciones.Entidades;
using Emplaniapp.AccesoADatos;
using Emplaniapp.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace Emplaniapp.UI
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configurar el contexto, administradores y signInManager para usar una única instancia por solicitud
            app.CreatePerOwinContext(Contexto.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Habilitar la aplicación para que use una cookie para almacenar información del usuario que ha iniciado sesión
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Habilita la validación de seguridad cuando el usuario inicia sesión.
                    // Es una característica de seguridad que invalida la cookie si el usuario cambia la contraseña.
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Habilita a la aplicación para que almacene temporalmente información del usuario cuando se verifica el segundo factor en el proceso de autenticación de dos factores.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Habilita a la aplicación para que recuerde el segundo factor de verificación de inicio de sesión, como el teléfono o el correo electrónico.
            // Una vez que active esta opción, se recordará el segundo paso de verificación durante el proceso de inicio de sesión en el dispositivo desde donde inició sesión.
            // Esto es similar a la opción Recordarme al iniciar sesión.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
        }

        private void SeedRoles()
        {
            using (var context = new Contexto())
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);

                string[] roles = { "Administrador", "Contador", "Empleado" };
                foreach (var role in roles)
                {
                    if (!manager.RoleExists(role))
                        manager.Create(new IdentityRole(role));
                }
            }
        }
    }
}
