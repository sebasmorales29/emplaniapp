using System.Web.Mvc;
using Emplaniapp.UI.Attributes;

namespace Emplaniapp.UI.Controllers
{
    [Authorize]
    public class PagoQuincenalController : Controller
    {
        // GET: PagoQuincenal/HistorialPagos/5
        [ActiveRoleAuthorize("Empleado")]
        public ActionResult HistorialPagos(int idEmpleado)
        {
            // Aquí iría la lógica para buscar y mostrar el historial de pagos del empleado.
            // Por ahora, solo pasamos el ID a la vista.
            ViewBag.IdEmpleado = idEmpleado;
            return View();
        }
    }
} 