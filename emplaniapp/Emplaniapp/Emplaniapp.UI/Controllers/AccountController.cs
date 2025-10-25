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

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
        }

        protected IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        // ========================= LOGIN =========================

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            // 1) Validar existencia de usuario
            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "El usuario ingresado no está registrado en el sistema.");
                return View(model);
            }

            // 2) Validar bloqueo por intentos
            if (await UserManager.IsLockedOutAsync(user.Id))
            {
                ModelState.AddModelError("", "Su cuenta está temporalmente bloqueada por múltiples intentos fallidos. Intente más tarde o contacte al administrador.");
                return View(model);
            }

            // 3) Validar estado del empleado
            using (var contexto = new Contexto())
            {
                var empleado = contexto.Empleados.FirstOrDefault(e => e.IdNetUser == user.Id);
                if (empleado == null)
                {
                    ModelState.AddModelError("", "Su cuenta no está asociada a ningún empleado. Contacte al administrador.");
                    return View(model);
                }

                // Estado 1 = Activo
                if (empleado.idEstado != 1)
                {
                    var estado = contexto.Estado.FirstOrDefault(e => e.idEstado == empleado.idEstado);
                    var nombreEstado = estado != null ? estado.nombreEstado : "Inactivo";
                    ModelState.AddModelError("", "Su cuenta está en estado '" + nombreEstado + "' y no puede iniciar sesión. Contacte al administrador.");
                    return View(model);
                }
            }

            // 4) Autenticar credenciales (permitimos lockout)
            var result = await SignInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                model.RememberMe,
                shouldLockout: true
            );

            switch (result)
            {
                case SignInStatus.Success:
                    {
                        // Crear identidad con claims
                        var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

                        using (var contexto = new Contexto())
                        {
                            var empleado = contexto.Empleados.FirstOrDefault(e => e.IdNetUser == user.Id);
                            if (empleado != null)
                            {
                                identity.AddClaim(new Claim("idEmpleado", empleado.idEmpleado.ToString()));
                            }
                        }

                        var roles = await UserManager.GetRolesAsync(user.Id);
                        if (roles == null || !roles.Any())
                        {
                            identity.AddClaim(new Claim("SinRol", "true"));
                            TempData["Mensaje"] = "Ha iniciado sesión, pero no posee ningún rol asignado. Contacte al administrador para asignar permisos.";
                            TempData["TipoMensaje"] = "warning";
                        }

                        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = model.RememberMe }, identity);
                        return RedirectToLocal(returnUrl);
                    }
                case SignInStatus.LockedOut:
                    ModelState.AddModelError("", "Su cuenta ha sido bloqueada temporalmente por múltiples intentos fallidos.");
                    return View(model);

                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });

                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "La contraseña ingresada es incorrecta.");
                    return View(model);
            }
        }

        // ================== RECUPERACIÓN DE CONTRASEÑA ==================

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await UserManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "El correo no existe en el sistema.");
                return View(model);
            }

            var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var callbackUrl = BuildResetPasswordUrl(user.Id, code); // <- SIN UrlEncode

            var safeUser = HttpUtility.HtmlEncode(user.UserName ?? user.Email);
            var body =
                "<p>Hola " + safeUser + ",</p>" +
                "<p>Has solicitado restablecer tu contraseña de <strong>Emplaniapp</strong>.</p>" +
                "<p>Puedes hacerlo dando clic en el siguiente enlace:</p>" +
                "<p><a href=\"" + callbackUrl + "\">Restablecer contraseña</a></p>" +
                "<p>Si no solicitaste este cambio, puedes ignorar este mensaje.</p>";

            await UserManager.SendEmailAsync(user.Id, "Restablecer contraseña", body);

            TempData["Ok"] = "Hemos enviado un correo con el enlace para restablecer tu contraseña.";
            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code, string userId)
        {
            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(userId))
            {
                ViewBag.ErrorMessage = "El enlace de restablecimiento es inválido o está incompleto. Solicita uno nuevo.";
                return View("ResetPasswordLinkError");
            }

            // Pasamos el token tal cual (sin decodificar)
            return View(new ResetPasswordViewModel { Code = code, UserId = userId });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await UserManager.FindByIdAsync(model.UserId);
            if (user == null)
                return RedirectToAction("ResetPasswordConfirmation");

            // Usamos el token tal cual (sin UrlDecode)
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
                return RedirectToAction("ResetPasswordConfirmation");

            foreach (var e in result.Errors)
            {
                ModelState.AddModelError("", e);
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // ========================= LOGOFF =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var userName = User.Identity.GetUserName();
                System.Diagnostics.Debug.WriteLine("LogOff: " + userName + " (" + userId + ")");

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login", "Account");
            }
            catch
            {
                try { AuthenticationManager.SignOut(); } catch { }
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

        // Construye URL absoluta para ResetPassword (SIN UrlEncode manual para evitar doble-encoding)
        private string BuildResetPasswordUrl(string userId, string rawCode)
        {
            var baseUrl = ConfigurationManager.AppSettings["PublicBaseUrl"];

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                var scheme = (Request != null && Request.Url != null && !string.IsNullOrWhiteSpace(Request.Url.Scheme))
                    ? Request.Url.Scheme
                    : "https";

                return Url.Action(
                    "ResetPassword",
                    "Account",
                    new { userId = userId, code = rawCode },   // <-- sin UrlEncode
                    protocol: scheme
                );
            }
            else
            {
                var relative = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { userId = userId, code = rawCode },   // <-- sin UrlEncode
                    protocol: null
                );
                return baseUrl.TrimEnd('/') + relative;
            }
        }
    }
}

