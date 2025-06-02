using System.Web.Mvc;

namespace Emplaniapp.UI.Controllers
{
    public class EmpleadoController : Controller
    {
        // GET: Empleado
        public ActionResult ListarEmpleados()
        {
            // Crear una lista de empleados (datos de ejemplo)

            return View();
        }

        // GET: Empleado/Details/5
        public ActionResult DetalleEmpleado(int id)
        {
            return View();
        }

        // GET: Empleado/Create
        public ActionResult CrearEmpleado()
        {
            return View();
        }

        // POST: Empleado/Create
        [HttpPost]
        public ActionResult CrearEmpleado(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Empleado/Edit/5
        public ActionResult EditarEmpleado(int id)
        {
            return View();
        }

        // POST: Empleado/Edit/5
        [HttpPost]
        public ActionResult EditarEmpleado(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Empleado/Delete/5
        public ActionResult EliminarEmpleado(int id)
        {
            return View();
        }

        // POST: Empleado/Delete/5
        [HttpPost]
        public ActionResult EliminarEmpleado(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
