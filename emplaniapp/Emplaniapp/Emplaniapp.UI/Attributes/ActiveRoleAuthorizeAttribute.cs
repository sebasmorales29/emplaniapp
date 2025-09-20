using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace Emplaniapp.UI.Attributes
{
    /// <summary>
    /// Atributo de autorización que verifica todos los roles del usuario (sistema unificado)
    /// </summary>
    public class ActiveRoleAuthorizeAttribute : AuthorizeAttribute
    {
        public ActiveRoleAuthorizeAttribute() { }

        public ActiveRoleAuthorizeAttribute(params string[] roles)
        {
            Roles = string.Join(",", roles);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            // Si no se especificaron roles, solo verificar autenticación
            if (string.IsNullOrEmpty(Roles))
            {
                return true;
            }

            // Obtener todos los roles del usuario desde Identity
            var claimsIdentity = httpContext.User.Identity as ClaimsIdentity;
            var userRoles = claimsIdentity?.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToArray();
            
            if (userRoles == null || !userRoles.Any())
            {
                return false;
            }

            // Verificar si el usuario tiene alguno de los roles permitidos
            var allowedRoles = SplitString(Roles);
            return allowedRoles.Any(allowedRole => 
                userRoles.Any(userRole => 
                    string.Equals(userRole, allowedRole, StringComparison.OrdinalIgnoreCase)));
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // Usuario autenticado pero sin el rol activo correcto
                var result = new ViewResult
                {
                    ViewName = "~/Views/Shared/UnauthorizedRole.cshtml"
                };
                filterContext.Result = result;
            }
            else
            {
                // Usuario no autenticado - redirigir al login
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

        private static string[] SplitString(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                return new string[0];
            }

            var split = from piece in original.Split(',')
                        let trimmed = piece.Trim()
                        where !string.IsNullOrEmpty(trimmed)
                        select trimmed;
            return split.ToArray();
        }
    }
}