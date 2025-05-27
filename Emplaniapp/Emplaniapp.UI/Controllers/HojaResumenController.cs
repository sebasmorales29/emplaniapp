using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Emplaniapp.UI.Controllers
{
    public class HojaResumenController : Controller
    {
        // GET: HojaResumen
        public ActionResult Index()
        {
            return View();
        }

        // GET: HojaResumen/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HojaResumen/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HojaResumen/Create
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

        // GET: HojaResumen/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HojaResumen/Edit/5
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

        // GET: HojaResumen/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HojaResumen/Delete/5
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



        // Vista ----------------------------

        // 1. Datos Personales
            // Cambiar datos
            // Verificar Contraseña
            // agregar datos -> admin 
            // activar/desactivar - admin


        // 2. Remuneraciones
            // listar remuneraicones
            // 


         // 3. Retenciones



        // 4. Observaciones


        // 5. Liquidación


    }
}
