using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Emplaniapp.UI.Controllers
{
    public class DatosPersonalesController : Controller
    {

        // Hacer un layout parcial según figma (propuesta 2)

        // 1. Datos Personales
        // Cambiar datos
        // Verificar Contraseña
        // agregar datos -> admin 
        // activar/desactivar - admin


        // GET: DatosPersonales
        public ActionResult Index()
        {
            return View();
        }

        // GET: DatosPersonales/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DatosPersonales/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DatosPersonales/Create
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

        // GET: DatosPersonales/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DatosPersonales/Edit/5
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

        // GET: DatosPersonales/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DatosPersonales/Delete/5
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
