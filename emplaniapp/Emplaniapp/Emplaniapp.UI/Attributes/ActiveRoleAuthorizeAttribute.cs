using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Emplaniapp.UI.Attributes
{
    /// <summary>
    /// Atributo de autorización que verifica el rol activo en lugar de todos los roles del usuario
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

            // Obtener el rol activo de la sesión
            var activeRole = httpContext.Session["ActiveRole"] as string;
            
            if (string.IsNullOrEmpty(activeRole))
            {
                return false;
            }

            // Verificar si el rol activo está en la lista de roles permitidos
            var allowedRoles = SplitString(Roles);
            return allowedRoles.Contains(activeRole, StringComparer.OrdinalIgnoreCase);
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