using Emplaniapp.Abstracciones.InterfacesParaUI;
using Emplaniapp.Abstracciones.InterfacesParaUI.Historial;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.LogicaDeNegocio.Historial;
using Emplaniapp.LogicaDeNegocio;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using Emplaniapp.LogicaDeNegocio.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.ObtenerEmpleadoPorId;
using System.IO;
using static System.Collections.Specialized.BitVector32;

namespace Emplaniapp.UI.Controllers
{
    public class ObservacionesController : Controller
    {
        private IObtenerEmpleadoPorIdLN _datosPersonalesLN;
        private IObservacionLN _observacionLN;
        private ApplicationUserManager _userManager;
        private IRegistrarEventoHistorialLN _registrarEventoHistorialLN;

        public ObservacionesController()
        {
            _observacionLN = new ObservacionLN();
            _datosPersonalesLN = new ObtenerEmpleadoPorIdLN();
            _registrarEventoHistorialLN = new RegistrarEventoHistorialLN();
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        public ActionResult Detalles(int? id, string seccion = "Observaciones")
        {
            var idEmpleado = id
                ?? int.Parse((User.Identity as ClaimsIdentity)?
                                 .FindFirst("idEmpleado")?.Value
                             ?? "0");

            var emp = _datosPersonalesLN.ObtenerEmpleadoPorId(idEmpleado);
            if (emp == null) return HttpNotFound();
            ViewBag.Seccion = seccion;
            return View(emp);
        }



        [HttpGet]
        public ActionResult ObtenerListaObservaciones(int id, int? ano, int? mes, string usuarioId)
        {
            var todasLasObservaciones = _observacionLN.ObtenerObservacionesPorEmpleado(id);

            ViewBag.AnosDisponibles = todasLasObservaciones
                .Select(o => o.FechaCreacion.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToList();

            ViewBag.MesesDisponibles = Enumerable.Range(1, 12).Select(m => new SelectListItem
            {
                Value = m.ToString(),
                Text = new DateTime(2000, m, 1).ToString("MMMM")
            }).ToList();

            var usuariosIds = todasLasObservaciones
                .Select(o => o.IdUsuarioCreo)
                .Union(todasLasObservaciones.Select(o => o.IdUsuarioEdito))
                .Where(uid => !string.IsNullOrEmpty(uid))
                .Distinct()
                .ToList();

            var usuariosDisponibles = new List<SelectListItem>();
            if (usuariosIds.Any())
            {
                usuariosDisponibles = UserManager.Users
                    .Where(u => usuariosIds.Contains(u.Id))
                    .Select(u => new SelectListItem { Value = u.Id, Text = u.UserName })
                    .ToList();
            }
            ViewBag.UsuariosDisponibles = usuariosDisponibles;

            ViewBag.AnoSeleccionado = ano;
            ViewBag.MesSeleccionado = mes;
            ViewBag.UsuarioSeleccionado = usuarioId;
            ViewBag.FiltrosActivos = ano.HasValue || mes.HasValue || !string.IsNullOrEmpty(usuarioId);

            var observacionesFiltradas = todasLasObservaciones;

            if (ano.HasValue)
                observacionesFiltradas = observacionesFiltradas.Where(o => o.FechaCreacion.Year == ano.Value).ToList();
            if (mes.HasValue)
                observacionesFiltradas = observacionesFiltradas.Where(o => o.FechaCreacion.Month == mes.Value).ToList();
            if (!string.IsNullOrEmpty(usuarioId))
                observacionesFiltradas = observacionesFiltradas.Where(o => o.IdUsuarioCreo == usuarioId || o.IdUsuarioEdito == usuarioId).ToList();

            var userIdsParaMostrar = observacionesFiltradas
                .Select(o => o.IdUsuarioCreo)
                .Concat(observacionesFiltradas.Select(o => o.IdUsuarioEdito))
                .Where(uid => !string.IsNullOrEmpty(uid))
                .Distinct()
                .ToList();

            var users = new Dictionary<string, string>();
            if (userIdsParaMostrar.Any())
            {
                users = UserManager.Users
                    .Where(u => userIdsParaMostrar.Contains(u.Id))
                    .ToDictionary(u => u.Id, u => u.UserName);
            }

            foreach (var obs in observacionesFiltradas)
            {
                if (!string.IsNullOrEmpty(obs.IdUsuarioCreo))
                {
                    obs.NombreUsuarioCreo = users.TryGetValue(obs.IdUsuarioCreo, out var userName) ? userName : "N/A";
                }
                if (!string.IsNullOrEmpty(obs.IdUsuarioEdito))
                {
                    obs.NombreUsuarioEdito = users.TryGetValue(obs.IdUsuarioEdito, out var userName) ? userName : "N/A";
                }
            }

            ViewBag.IdEmpleado = id;
            string partialViewHtml = RenderRazorViewToString("_ObservacionesList", observacionesFiltradas);
            return Json(new { success = true, html = partialViewHtml }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AgregarObservacion(int idEmpleado)
        {
            var model = new ObservacionDto
            {
                IdEmpleado = idEmpleado
            };
            return PartialView("_ObservacionForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarObservacion(ObservacionDto model)
        {
            if (ModelState.IsValid)
            {
                model.IdUsuarioCreo = User.Identity.GetUserId();
                model.FechaCreacion = DateTime.Now;

                bool resultado = _observacionLN.GuardarObservacion(model);

                if (resultado)
                {
                    try
                    {
                        _registrarEventoHistorialLN.RegistrarEvento(
                            model.IdEmpleado,
                            "Agregar Observación",
                            "Se agregó una nueva observación al empleado",
                            $"Observación: {model.Descripcion}",
                            null,
                            null,
                            User.Identity.GetUserId(),
                            Request.UserHostAddress
                        );
                    }
                    catch { /* log */ }

                    return Json(new { success = true });
                }
                ModelState.AddModelError("", "Ocurrió un error al guardar la observación.");
            }
            return PartialView("_ObservacionForm", model);
        }

        [HttpGet]
        public ActionResult EditarObservacion(int id)
        {
            var model = _observacionLN.ObtenerObservacionPorId(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return PartialView("_ObservacionForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarObservacion(ObservacionDto model)
        {
            if (ModelState.IsValid)
            {
                model.IdUsuarioEdito = User.Identity.GetUserId();
                model.FechaEdicion = DateTime.Now;

                bool resultado = _observacionLN.ActualizarObservacion(model);

                if (resultado)
                {
                    try
                    {
                        _registrarEventoHistorialLN.RegistrarEvento(
                            model.IdEmpleado,
                            "Modificar Observación",
                            "Se modificó una observación del empleado",
                            $"Observación modificada: {model.Descripcion}",
                            null,
                            null,
                            User.Identity.GetUserId(),
                            Request.UserHostAddress
                        );
                    }
                    catch { /* log */ }

                    return Json(new { success = true });
                }
                ModelState.AddModelError("", "Ocurrió un error al guardar los cambios.");
            }
            return PartialView("_ObservacionForm", model);
        }


        protected string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }




    }
}
