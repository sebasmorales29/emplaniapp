using Emplaniapp.Abstracciones.InterfacesParaUI;
using Emplaniapp.Abstracciones.InterfacesParaUI.Retenciones;
using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.LogicaDeNegocio;
using Emplaniapp.LogicaDeNegocio.Retenciones;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Emplaniapp.UI.Controllers
{
    [Authorize]
    public class RetencionesController : Controller
    {
        private IListarRetencionesLN _listarReten;
        private IDatosPersonalesLN _datosPersonalesLN;
        private ApplicationUserManager _userManager;

        // Constructores --------------------------------------------------------------
        public RetencionesController()
        {
            _listarReten = new ListarRetencionesLN();
            _datosPersonalesLN = new DatosPersonalesLN();
        }

        // Constructor dependencias (Identity/OWIN) 
         public RetencionesController(ApplicationUserManager userManager)
        {
            _listarReten = new ListarRetencionesLN();
            _datosPersonalesLN = new DatosPersonalesLN();
            UserManager = userManager;
        }
        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }



        // Listar Retenciones -------------------------------------------------------

        public ActionResult Detalles(int? id, string seccion = "Retenciones")
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
                <EmpleadoDto, List<RetencionDto>>
                (empleado, _listarReten.Listar(idEmpleado));

            ViewBag.Seccion = seccion;
            return View(variables);

        }





        // GET: Retenciones
        public ActionResult Lista(int idEmpleado)
        {
            List<RetencionDto> lista = _listarReten.Listar(idEmpleado);
            return View(lista);
        }

        // GET: Retenciones/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Retenciones/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Retenciones/Create
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

        // GET: Retenciones/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Retenciones/Edit/5
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

        // GET: Retenciones/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Retenciones/Delete/5
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
