using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Emplaniapp.Abstracciones.InterfacesParaUI;
using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones;
using Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones.CrearRemuneraciones;
using Emplaniapp.Abstracciones.InterfacesParaUI.Tipo_Remuneracion;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.LogicaDeNegocio;
using Emplaniapp.LogicaDeNegocio.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.LogicaDeNegocio.Remuneraciones;
using Emplaniapp.LogicaDeNegocio.Remuneraciones.CrearRemuneraciones;
using Emplaniapp.LogicaDeNegocio.Tipo_Remuneracion;
using Microsoft.AspNet.Identity.Owin;

namespace Emplaniapp.UI.Controllers
{
    [Authorize]
    public class RemuneracionesController : Controller
    {

        private IListarRemuneracionesLN _listarRemu;
        private IDatosPersonalesLN _datosPersonalesLN;
        private ICrearRemuneracionesLN _crearRemuneracionesLN;
        private IListarTipoRemuneracionLN _listarTipoRemuneracionLN;
        private IObtenerEmpleadoPorIdLN _obtenerEmpleadoPorIdLN;
        private ApplicationUserManager _userManager;

        // Constructores ------------------------------------------------------------------------------
        public RemuneracionesController()
        {
            _listarRemu = new ListarRemuneracionesLN();
            _datosPersonalesLN = new DatosPersonalesLN();
            _crearRemuneracionesLN = new CrearRemuneracionesLN();
            _listarTipoRemuneracionLN = new ListarTipoRemuneracionLN();
            _obtenerEmpleadoPorIdLN = new ObtenerEmpleadoPorIdLN();
        }



        // Constructor para inyección de dependencias (usado por Identity/OWIN)
        public RemuneracionesController(ApplicationUserManager userManager)
        {
            _listarRemu = new ListarRemuneracionesLN();
            _datosPersonalesLN = new DatosPersonalesLN();
            _crearRemuneracionesLN = new CrearRemuneracionesLN();
            _listarTipoRemuneracionLN = new ListarTipoRemuneracionLN();
            _obtenerEmpleadoPorIdLN = new ObtenerEmpleadoPorIdLN();
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

            ViewBag.TiposRemuneracion = _listarTipoRemuneracionLN.Listar()
           .Select(t => new SelectListItem
           {
               Value = t.Id.ToString(),
               Text = t.nombreTipoRemuneracion
           })
           .ToList();

            var empleado = _datosPersonalesLN.ObtenerEmpleadoPorId(idEmpleado);

            if (empleado == null)
            {
                return HttpNotFound();
            }

            ViewBag.TiposRemuneracion = ObtenerTiposRemuneracion();

            var variables = new Tuple
                <EmpleadoDto, List<RemuneracionDto>>
                (empleado,_listarRemu.Listar(idEmpleado));

            ViewBag.SeccionRemu = seccion;
            return View(variables);

        }



        // AGREGAR REMUNERACIÓN ------------------------------------------
        private List<SelectListItem> ObtenerTiposRemuneracion()
        {
            var todosTipos = _listarTipoRemuneracionLN.Listar()
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.nombreTipoRemuneracion
                })
                .ToList();

            return todosTipos; // ← Esto faltaba
        }

        // GET: Remuneraciones/Create
        [HttpGet]
        public ActionResult _CrearRemuneracionManual(int idEmpleado)
        {
            try
            {
                // 1. Obtener tipos de remuneración
                var tiposRemuneracion = _listarTipoRemuneracionLN.Listar();

                if (tiposRemuneracion == null || !tiposRemuneracion.Any())
                {
                    tiposRemuneracion = new List<TipoRemuneracionDto>(); // Lista vacía si no hay datos
                }

                // 2. Convertir a SelectListItem
                var items = tiposRemuneracion
                    .Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.nombreTipoRemuneracion ?? "Sin nombre"
                    })
                    .ToList();

                // 3. Asignar al ViewBag
                ViewBag.TiposRemuneracion = items;

                // 4. Crear modelo
                var model = new RemuneracionDto
                {
                    idEmpleado = idEmpleado,
                    fechaRemuneracion = DateTime.Today
                };

                return PartialView("_CrearRemuneracionManual", model);
            }
            catch (Exception ex)
            {
                // Loggear el error
                // _logger.LogError(ex, "Error al cargar formulario de remuneración");

                // Retornar lista vacía en caso de error
                ViewBag.TiposRemuneracion = new List<SelectListItem>();

                return PartialView("_CrearRemuneracionManual", new RemuneracionDto
                {
                    idEmpleado = idEmpleado
                });
            }
        }

        // POST: Remuneraciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _CrearRemuneracionManual(RemuneracionDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _crearRemuneracionesLN.AgregarRemuneracionManual(model, model.idEmpleado);
                    TempData["Mensaje"] = "Remuneración agregada correctamente.";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                    return PartialView("_CrearRemuneracionManual", model); // Si quieres que muestre errores en modal sin recargar página
                }
                return RedirectToAction("DetallesRemu", new { id = model.idEmpleado });
            }
            else
            {
                var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ErrorMessage"] = string.Join("; ", errores);
                return PartialView("_CrearRemuneracionManual", model); // Mostrar errores en modal
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
