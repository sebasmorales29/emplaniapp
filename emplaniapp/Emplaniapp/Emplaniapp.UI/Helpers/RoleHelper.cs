using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace Emplaniapp.UI.Helpers
{
    public static class RoleHelper
    {
        /// <summary>
        /// Verifica si el usuario está en el rol activo especificado
        /// </summary>
        public static bool IsInActiveRole(string role)
        {
            var activeRole = GetActiveRole();
            return activeRole.Equals(role, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Verifica si el usuario está en cualquiera de los roles activos especificados
        /// </summary>
        public static bool IsInAnyActiveRole(string[] roles)
        {
            var activeRole = GetActiveRole();
            return Array.Exists(roles, role => activeRole.Equals(role, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Verifica si el usuario NO está en el rol activo especificado
        /// </summary>
        public static bool IsNotInActiveRole(string role)
        {
            return !IsInActiveRole(role);
        }

        /// <summary>
        /// Obtiene el rol activo actual del usuario
        /// </summary>
        public static string GetActiveRole()
        {
            var activeRole = HttpContext.Current.Session["ActiveRole"] as string;
            if (!string.IsNullOrEmpty(activeRole))
            {
                return activeRole;
            }

            // Fallback al rol original del usuario
            var user = HttpContext.Current.User;
            if (user.Identity.IsAuthenticated)
            {
                var claimsIdentity = user.Identity as ClaimsIdentity;
                var roleClaim = claimsIdentity?.FindFirst(ClaimTypes.Role);
                if (roleClaim != null)
                {
                    return roleClaim.Value;
                }
            }

            return "Usuario"; // Valor por defecto
        }
    }
} 