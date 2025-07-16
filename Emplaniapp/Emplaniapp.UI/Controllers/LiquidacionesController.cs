using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.Abstracciones.InterfacesParaUI.Liquidaciones;
using Emplaniapp.LogicaDeNegocio.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.LogicaDeNegocio.Liquidaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Emplaniapp.UI.Controllers
{
    [Authorize]
    public class LiquidacionesController : Controller
    {
        private readonly IObtenerEmpleadoPorIdLN _obtenerEmpleado;
        private readonly IMostrarCalculosPreviosLiqLN _calculosPrevios;
        
        public LiquidacionesController()
        {
            _obtenerEmpleado = new ObtenerEmpleadoPorIdLN();
            _calculosPrevios = new MostrarCalculosPreviosLiqLN();   
        }

        public ActionResult Detalles(int? id, string seccion = "Liquidacion")
        {
            int idEmpleado = id ?? int.Parse(
                (User.Identity as ClaimsIdentity)?.FindFirst("idEmpleado")?.Value ?? "0"
            );

            var emp = _obtenerEmpleado.ObtenerEmpleadoPorId(idEmpleado);
            if (emp == null) return HttpNotFound();

            var liq = _calculosPrevios.MostrarLiquidacion(idEmpleado);
            // hacer un obtener liquidacion para que, si es nulo, entonces presente el mostrar                

            ViewBag.Seccion = seccion;
            return View(Tuple.Create(emp, liq));
        }




        // GET: Liquidaciones
        public ActionResult Index()
        {
            return View();
        }

        // GET: Liquidaciones/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Liquidaciones/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Liquidaciones/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
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

        // GET: Liquidaciones/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Liquidaciones/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
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

        // GET: Liquidaciones/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Liquidaciones/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
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
