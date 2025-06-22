using Emplaniapp.Abstracciones.InterfacesParaUI;
using Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.LogicaDeNegocio;
using Emplaniapp.LogicaDeNegocio.Remuneraciones;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Emplaniapp.UI.Controllers
{
    [Authorize]
    public class RemuneracionesController : Controller
    {

        private IListarRemuneracionesLN _listarRemu;
        private IDatosPersonalesLN _datosPersonalesLN;
        private ApplicationUserManager _userManager;

        // Constructores ------------------------------------------------------------------------------
        public RemuneracionesController()
        {
            _listarRemu = new ListarRemuneracionesLN();
            _datosPersonalesLN = new DatosPersonalesLN();
        }



        // Constructor para inyección de dependencias (usado por Identity/OWIN)
        public RemuneracionesController(ApplicationUserManager userManager)
        {
            _listarRemu = new ListarRemuneracionesLN();
            _datosPersonalesLN = new DatosPersonalesLN();
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }


        // LISTA DE REMUNERACIONES ---------------------------------
        public ActionResult DetallesRemu(int? id, string seccion = "Remuneraciones")
        {
            int idEmpleado;

            if (id.HasValue)
            {
                // Si se provee un ID en la URL (ej: admin viendo un perfil)
                idEmpleado = id.Value;
            }
            else
            {
                // Si no hay ID, buscarlo en las claims del usuario (ej: empleado viendo su propio perfil)
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var idEmpleadoClaim = claimsIdentity?.FindFirst("idEmpleado");

                if (idEmpleadoClaim == null || !int.TryParse(idEmpleadoClaim.Value, out idEmpleado))
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "No se pudo identificar al empleado.");
                }
            }

            var empleado = _datosPersonalesLN.ObtenerEmpleadoPorId(idEmpleado);

            if (empleado == null)
            {
                return HttpNotFound();
            }

            var variables = new Tuple
                <EmpleadoDto, List<RemuneracionDto>>
                (empleado,_listarRemu.Listar(idEmpleado));

            ViewBag.SeccionRemu = seccion;
            return View(variables);

        }



        // AGREGAR REMUNERACIÓN ------------------------------------------

        // GET: Remuneraciones/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Remuneraciones/Create
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


        // EDITAR REMUNERACIÓN ------------------------------------------
        
        // GET: Remuneraciones/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Remuneraciones/Edit/5
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


        // ELIMINAR REMUNERACIÓN ------------------------------------------

        // GET: Remuneraciones/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Remuneraciones/Delete/5
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
