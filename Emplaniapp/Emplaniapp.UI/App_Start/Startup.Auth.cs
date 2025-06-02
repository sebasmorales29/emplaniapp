using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Emplaniapp.UI.Models;

namespace Emplaniapp.UI
{
    // Nota: cambiamos la clase a partial y quitamos el atributo [OwinStartup] aquí.
    public partial class Startup
    {
        // Este método es invocado desde Startup.cs
        public void ConfigureAuth(IAppBuilder app)
        {
            // 1) Registrar el contexto de Identity y los managers
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // 2) Configurar la autenticación mediante Cookies
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                ExpireTimeSpan = TimeSpan.FromMinutes(30),
                SlidingExpiration = true
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // 3) (Opcional) Proveedores externos:
            // app.UseGoogleAuthentication(...);
            // app.UseFacebookAuthentication(...);
        }
    }
}
