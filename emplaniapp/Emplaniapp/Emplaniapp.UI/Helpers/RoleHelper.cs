using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace Emplaniapp.UI.Helpers
{
    public static class RoleHelper
    {
        /// <summary>
        /// Verifica si el usuario tiene el rol especificado (sistema unificado)
        /// </summary>
        public static bool HasRole(string role)
        {
            var userRoles = GetAllUserRoles();
            return userRoles.Contains(role, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Verifica si el usuario tiene alguno de los roles especificados
        /// </summary>
        public static bool HasAnyRole(string[] roles)
        {
            var userRoles = GetAllUserRoles();
            return roles.Any(role => userRoles.Contains(role, StringComparer.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Verifica si el usuario NO tiene el rol especificado
        /// </summary>
        public static bool DoesNotHaveRole(string role)
        {
            return !HasRole(role);
        }

        /// <summary>
        /// Obtiene todos los roles del usuario actual
        /// </summary>
        public static string[] GetAllUserRoles()
        {
            var user = HttpContext.Current.User;
            if (!user.Identity.IsAuthenticated)
            {
                return new string[0];
            }

            var claimsIdentity = user.Identity as ClaimsIdentity;
            var roleClaims = claimsIdentity?.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToArray();

            return roleClaims ?? new string[0];
        }

        /// <summary>
        /// Obtiene el rol principal del usuario (el de mayor prioridad)
        /// </summary>
        public static string GetPrimaryRole()
        {
            var userRoles = GetAllUserRoles();
            if (!userRoles.Any())
            {
                return "Usuario";
            }

            // Prioridad: Administrador > Contador > Empleado
            if (userRoles.Contains("Administrador", StringComparer.OrdinalIgnoreCase))
                return "Administrador";
            if (userRoles.Contains("Contador", StringComparer.OrdinalIgnoreCase))
                return "Contador";
            if (userRoles.Contains("Empleado", StringComparer.OrdinalIgnoreCase))
                return "Empleado";

            return userRoles.First();
        }

        /// <summary>
        /// Verifica si el usuario tiene múltiples roles
        /// </summary>
        public static bool HasMultipleRoles()
        {
            return GetAllUserRoles().Length > 1;
        }
    }
} 