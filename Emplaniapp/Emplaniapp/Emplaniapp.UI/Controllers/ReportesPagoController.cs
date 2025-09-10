using System.Security.Claims;
using System.Web.Mvc;

namespace Emplaniapp.UI.Controllers
{
    [Authorize] // Empleado/Contador/Administrador
    public class ReportesPagoController : Controller
    {
        [HttpGet]
        public ActionResult HistorialPagos(int? id)
        {
            // Admin/Contador: pueden pasar ?id=123 para ver los datos financieros de ese empleado
            if (id.HasValue && (User.IsInRole("Administrador") || User.IsInRole("Contador")))
            {
                return RedirectToAction("Detalles", "DatosPersonales", new { id = id.Value, seccion = "Datos financieros" });
            }

            // Empleado: usar su idEmpleado desde las claims
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var idEmpleadoClaim = claimsIdentity?.FindFirst("idEmpleado");

            int idEmpleado;
            if (idEmpleadoClaim != null && int.TryParse(idEmpleadoClaim.Value, out idEmpleado))
            {
                return RedirectToAction("Detalles", "DatosPersonales", new { id = idEmpleado, seccion = "Datos financieros" });
            }

            TempData["ErrorMessage"] = "No se pudo identificar al empleado asociado a tu usuario.";
            // redirige igual a la sección de datos financieros (el controlador intenta resolver el id por claims)
            return RedirectToAction("Detalles", "DatosPersonales", new { seccion = "Datos financieros" });
        }
    }
}
