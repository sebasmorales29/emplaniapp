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

            var result = await SignInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                model.RememberMe,
                shouldLockout: true
            );

            switch (result)
            {
                case SignInStatus.Success:
                    var user = await UserManager.FindByNameAsync(model.UserName);
                    if (user != null)
                    {
                        var originalIdentity = await UserManager.CreateIdentityAsync(
                            user, DefaultAuthenticationTypes.ApplicationCookie);

                        using (var contexto = new Contexto())
                        {
                            var empleado = contexto.Empleados.FirstOrDefault(e => e.IdNetUser == user.Id);
                            if (empleado != null)
                            {
                                originalIdentity.AddClaim(new Claim("idEmpleado", empleado.idEmpleado.ToString()));
                                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                                AuthenticationManager.SignIn(
                                    new AuthenticationProperties { IsPersistent = model.RememberMe },
                                    originalIdentity);
                            }
                        }
                    }
                    return RedirectToLocal(returnUrl);

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
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
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