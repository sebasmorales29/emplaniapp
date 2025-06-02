using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado;
using Emplaniapp.LogicaDeNegocio.Empleado;

namespace Emplaniapp.UI.Controllers
{
    public class HojaResumenController : Controller
    {
        // Servicios
        private IEmpleadoLN _empleadoLN;

        public HojaResumenController()
        {
            _empleadoLN = new EmpleadoLN();
        }

        // GET: HojaResumen
        public ActionResult Index()
        {
            try
            {
                // Obtener los empleados reales de la base de datos
                var empleados = _empleadoLN.ListarTodos();
                return View(empleados);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar los empleados: " + ex.Message;
                return View(new List<EmpleadoDto>());
            }
        }

        // GET: HojaResumen/VerDetalles/5
        public ActionResult VerDetalles(int id)
        {
            // Redirige al controlador de DatosPersonales para ver los detalles del empleado
            return RedirectToAction("Detalles", "DatosPersonales", new { id = id });
        }

        // GET: HojaResumen/Create
        public ActionResult Create()
        {
            // Redirige al controlador de DatosPersonales para crear un empleado
            return RedirectToAction("Create", "DatosPersonales");
        }

        // POST: HojaResumen/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            // Redirige al controlador de DatosPersonales
            return RedirectToAction("Create", "DatosPersonales");
        }

        // GET: HojaResumen/Edit/5
        public ActionResult Edit(int id)
        {
            // Redirige al controlador de DatosPersonales para editar un empleado
            return RedirectToAction("Edit", "DatosPersonales", new { id = id });
        }

        // POST: HojaResumen/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            // Redirige al controlador de DatosPersonales
            return RedirectToAction("Edit", "DatosPersonales", new { id = id });
        }

        // GET: HojaResumen/Delete/5
        public ActionResult Delete(int id)
        {
            // Redirige al controlador de DatosPersonales para eliminar un empleado
            return RedirectToAction("Delete", "DatosPersonales", new { id = id });
        }

        // POST: HojaResumen/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            // Redirige al controlador de DatosPersonales
            return RedirectToAction("Delete", "DatosPersonales", new { id = id });
        }
    }
}
