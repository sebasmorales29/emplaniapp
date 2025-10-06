using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Emplaniapp.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using Emplaniapp.AccesoADatos;

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

        protected IAuthenticationManager AuthenticationManager =>
            HttpContext.GetOwinContext().Authentication;

        // ========================= LOGIN =========================

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            // ✨ MEJORA: Verificar si el usuario existe antes de intentar el login
            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                // ✨ Usuario no existe
                ModelState.AddModelError("", "❌ El usuario ingresado no está registrado en el sistema. Por favor, verifique el nombre de usuario.");
                return View(model);
            }

            // ✨ Verificar si el usuario está bloqueado
            if (await UserManager.IsLockedOutAsync(user.Id))
            {
                ModelState.AddModelError("", "🔒 Su cuenta está temporalmente bloqueada debido a múltiples intentos fallidos. Por favor, espere unos minutos o contacte al administrador.");
                return View(model);
            }

            // ✨ COMENTADO: Verificación de email confirmado no requerida para empleados internos
            // if (!await UserManager.IsEmailConfirmedAsync(user.Id))
            // {
            //     ModelState.AddModelError("", "📧 Su cuenta no ha sido verificada. Por favor, revise su correo electrónico y confirme su cuenta antes de iniciar sesión.");
            //     return View(model);
            // }

            var result = await SignInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                model.RememberMe,
                shouldLockout: true
            );

            switch (result)
            {
                case SignInStatus.Success:
                    if (user != null)
                    {
                        // ✨ MEJORA: Verificar estado del empleado y roles antes de permitir login
                        using (var contexto = new Contexto())
                        {
                            var empleado = contexto.Empleados.FirstOrDefault(e => e.IdNetUser == user.Id);
                            
                            // ✨ Verificar si el empleado existe
                            if (empleado == null)
                            {
                                ModelState.AddModelError("", "❌ Su cuenta no está asociada a ningún empleado. Por favor, contacte al administrador.");
                                return View(model);
                            }
                            
                            // ✨ Verificar si el empleado está activo (estado 1 = Activo)
                            if (empleado.idEstado != 1)
                            {
                                var estado = contexto.Estado.FirstOrDefault(e => e.idEstado == empleado.idEstado);
                                var nombreEstado = estado?.nombreEstado ?? "Inactivo";
                                
                                ModelState.AddModelError("", $"🔒 Su cuenta está en estado '{nombreEstado}' y no puede iniciar sesión. Por favor, contacte al administrador para más información.");
                                return View(model);
                            }
                            
                            // ✨ Verificar si el usuario tiene roles asignados
                            var userRoles = await UserManager.GetRolesAsync(user.Id);
                            if (userRoles == null || !userRoles.Any())
                            {
                                // ✨ Usuario sin roles - permitir login pero mostrar mensaje
                                var originalIdentity = await UserManager.CreateIdentityAsync(
                                    user, DefaultAuthenticationTypes.ApplicationCookie);
                                
                                originalIdentity.AddClaim(new Claim("idEmpleado", empleado.idEmpleado.ToString()));
                                originalIdentity.AddClaim(new Claim("SinRol", "true")); // Marcar que no tiene roles
                                
                                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                                AuthenticationManager.SignIn(
                                    new AuthenticationProperties { IsPersistent = model.RememberMe },
                                    originalIdentity);
                                
                                // ✨ Redirigir con mensaje informativo
                                TempData["Mensaje"] = "⚠️ Ha iniciado sesión correctamente, pero no posee ningún rol asignado. Por favor, contacte al administrador para asignar los permisos correspondientes.";
                                TempData["TipoMensaje"] = "warning";
                                return RedirectToLocal(returnUrl);
                            }
                            
                            // ✨ Usuario activo con roles - login normal
                            var identity = await UserManager.CreateIdentityAsync(
                                user, DefaultAuthenticationTypes.ApplicationCookie);
                            
                            identity.AddClaim(new Claim("idEmpleado", empleado.idEmpleado.ToString()));
                            
                            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                            AuthenticationManager.SignIn(
                                new AuthenticationProperties { IsPersistent = model.RememberMe },
                                identity);
                        }
                    }
                    return RedirectToLocal(returnUrl);

                case SignInStatus.LockedOut:
                    ModelState.AddModelError("", "🔒 Su cuenta ha sido bloqueada temporalmente debido a múltiples intentos fallidos. Por favor, espere 5 minutos o contacte al administrador.");
                    return View(model);

                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });

                case SignInStatus.Failure:
                default:
                    // ✨ Contraseña incorrecta - mensaje específico
                    ModelState.AddModelError("", "🔑 La contraseña ingresada es incorrecta. Por favor, verifique su contraseña e intente nuevamente.");
                    return View(model);
            }
        }

        // ================== RECUPERACIÓN DE CONTRASEÑA ==================

        [AllowAnonymous]
        public ActionResult ForgotPassword() => View();

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await UserManager.FindByEmailAsync(model.Email);

            // No revelamos si el usuario existe o no
            if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)) )
                return RedirectToAction("ForgotPasswordConfirmation");

            var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

            // Construye URL con host/puerto reales del request (opción 2)
            var callbackUrl = BuildResetPasswordUrl(user.Id, code);

            var safeUser = HttpUtility.HtmlEncode(user.UserName ?? user.Email);
            var body = $@"
                <p>Hola {safeUser},</p>
                <p>Has solicitado restablecer tu contraseña de <strong>Emplaniapp</strong>.</p>
                <p>Puedes hacerlo dando clic en el siguiente enlace:</p>
                <p><a href=""{callbackUrl}"">Restablecer contraseña</a></p>
                <p>Si tú no solicitaste este cambio, por favor ignora este mensaje.</p>";

            await UserManager.SendEmailAsync(user.Id, "Restablecer contraseña", body);

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation() => View();

        [AllowAnonymous]
        public ActionResult ResetPassword(string code, string userId)
        {
            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(userId))
                return View("Error");

            // IMPORTANTE: decodificar el token que llegó por querystring
            var decoded = HttpUtility.UrlDecode(code);
            return View(new ResetPasswordViewModel { Code = decoded, UserId = userId });
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await UserManager.FindByIdAsync(model.UserId);
            if (user == null)
                return RedirectToAction("ResetPasswordConfirmation");

            // IMPORTANTE: decodificar también lo que viene del formulario
            var decodedCode = HttpUtility.UrlDecode(model.Code);

            var result = await UserManager.ResetPasswordAsync(user.Id, decodedCode, model.Password);
            if (result.Succeeded)
                return RedirectToAction("ResetPasswordConfirmation");

            foreach (var e in result.Errors)
                ModelState.AddModelError("", e);

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation() => View();

        // ========================= LOGOFF =========================

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("🔍 DIAGNÓSTICO: LogOff iniciado");
                
                // Obtener información del usuario antes de cerrar sesión
                var userId = User.Identity.GetUserId();
                var userName = User.Identity.GetUserName();
                System.Diagnostics.Debug.WriteLine($"🔍 DIAGNÓSTICO: Cerrando sesión para usuario: {userName} (ID: {userId})");
                
                // Cerrar sesión
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                
                System.Diagnostics.Debug.WriteLine("🔍 DIAGNÓSTICO: Sesión cerrada exitosamente, redirigiendo a Login");
                
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ ERROR en LogOff: {ex.Message}");
                // En caso de error, intentar cerrar sesión de todas formas y redirigir
                try
                {
                    AuthenticationManager.SignOut();
                }
                catch { }
                return RedirectToAction("Login", "Account");
            }
        }

        // ========================= HELPERS =========================

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        // Construye URL absoluta para ResetPassword
        private string BuildResetPasswordUrl(string userId, string rawCode)
        {
            var encodedCode = HttpUtility.UrlEncode(rawCode ?? string.Empty);

            var baseUrl = ConfigurationManager.AppSettings["PublicBaseUrl"];
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                return Url.Action(
                    "ResetPassword",
                    "Account",
                    new { userId = userId, code = encodedCode },
                    protocol: Request?.Url?.Scheme
                );
            }
            else
            {
                var relative = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { userId = userId, code = encodedCode },
                    protocol: null
                );
                return baseUrl.TrimEnd('/') + relative;
            }
        }
    }
}