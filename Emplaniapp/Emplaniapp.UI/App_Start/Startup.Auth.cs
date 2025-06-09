using System;
using Emplaniapp.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace Emplaniapp.UI
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // 1) Contexto, UserManager, SignInManager, RoleManager
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            // 2) Cookie auth
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                ExpireTimeSpan = TimeSpan.FromMinutes(30),
                SlidingExpiration = true
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // 3) Seed de roles sin HttpContext
            SeedRoles();
        }

        private void SeedRoles()
        {
            using (var context = new ApplicationDbContext())
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
