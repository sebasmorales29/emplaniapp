using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Emplaniapp.Abstracciones.Entidades;
using Emplaniapp.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using Emplaniapp.AccesoADatos;
using Emplaniapp.Abstracciones.ModelosAD;

namespace Emplaniapp.UI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

        public AccountController() { }

        public AccountController(
            ApplicationUserManager userManager,
            ApplicationSignInManager signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public ApplicationUserManager UserManager =>
            _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        public ApplicationSignInManager SignInManager =>
            _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();

        private IAuthenticationManager AuthenticationManager =>
            HttpContext.GetOwinContext().Authentication;

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            // --- CÓDIGO DE DIAGNÓSTICO TEMPORAL ---
            // Descomente esta línea para generar un nuevo hash si la contraseña del admin no funciona.
            // var newHash = UserManager.PasswordHasher.HashPassword("Password123.");
            // System.Diagnostics.Debug.WriteLine("NUEVO HASH PARA 'Password123.': " + newHash);
            // --- FIN CÓDIGO DE DIAGNÓSTICO ---

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // --- INICIO DE CÓDIGO DE DEPURACIÓN ---
            System.Diagnostics.Debug.WriteLine($"Intento de inicio de sesión para el usuario: '{model.UserName}'.");
            var userForCheck = await UserManager.FindByNameAsync(model.UserName);
            if (userForCheck == null)
            {
                System.Diagnostics.Debug.WriteLine($"Resultado: El usuario '{model.UserName}' NO FUE ENCONTRADO en la base de datos.");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Resultado: Usuario '{model.UserName}' ENCONTRADO. Verificando contraseña...");
            }
            // --- FIN DE CÓDIGO DE DEPURACIÓN ---

            // Usar SignInManager para gestionar el inicio de sesión.
            // Esto maneja todos los casos: usuario no encontrado, contraseña incorrecta, lockout, etc.
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            
            // --- INICIO DE CÓDIGO DE DEPURACIÓN ---
            System.Diagnostics.Debug.WriteLine($"Resultado de PasswordSignInAsync: {result}");
            // --- FIN DE CÓDIGO DE DEPURACIÓN ---

            switch (result)
            {
                case SignInStatus.Success:
                    var user = await UserManager.FindByNameAsync(model.UserName);
                    if (user == null)
                    {
                        // Esto no debería ocurrir si SignInStatus.Success, pero es una salvaguarda.
                        ModelState.AddModelError("", "Error inesperado al iniciar sesión.");
                        return View(model);
                    }

                    // --- LÓGICA DE ACCESO BASADA EN ROLES (CORREGIDA) ---
                    var roles = await UserManager.GetRolesAsync(user.Id);
                    bool esAdminOContador = roles.Contains("Administrador") || roles.Contains("Contador");

                    using (var db = new Contexto())
                    {
                        var empleado = db.Empleados.FirstOrDefault(e => e.IdNetUser == user.Id);

                        // Administradores y Contadores siempre tienen acceso.
                        // Otros roles (como Empleado) solo si su registro de empleado existe y está activo.
                        bool tieneAcceso = esAdminOContador || (empleado != null && empleado.idEstado == 1);

                        if (tieneAcceso)
                        {
                            // Una vez validado, creamos una identidad enriquecida con nuestros claims.
                            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie); // Limpiamos la cookie de SignInManager.
                            
                            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                            
                            // Agregamos el idEmpleado al "pasaporte" (claim) del usuario.
                            if (empleado != null)
                            {
                                identity.AddClaim(new Claim("idEmpleado", empleado.idEmpleado.ToString()));
                            }
                            
                            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = model.RememberMe }, identity);

                            return RedirectToLocal(returnUrl);
                        }
                    }
                    
                    // Si el empleado no está activo o no existe, nos aseguramos de desconectar y mostramos el error.
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    ModelState.AddModelError("", "El usuario no está activo o no tiene acceso.");
                    return View(model);

                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                    return View(model);
            }
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            var rm = HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            var model = new RegisterViewModel
            {
                RolesList = rm.Roles
                    .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                    .ToList()
            };
            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // recarga RolesList antes de devolver la vista
                var rm = HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
                model.RolesList = rm.Roles
                    .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                    .ToList();
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email
            };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Asignar el rol elegido
                await UserManager.AddToRoleAsync(user.Id, model.Role);

                // Enlazar el IdNetUser con el Empleado recién creado
                using (var db = new Contexto())
                {
                    var empleado = db.Empleados.FirstOrDefault(e => e.cedula == model.Cedula);
                    if (empleado != null)
                    {
                        empleado.IdNetUser = user.Id;
                        await db.SaveChangesAsync();
                    }
                }
                
                //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                return RedirectToAction("Index", "Home");
            }

            // Si falla, anexa errores y recarga RolesList
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);

            var rm2 = HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            model.RolesList = rm2.Roles
                .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                .ToList();

            return View(model);
        }

        #region Helpers

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);
        }

        #endregion
    }
}
