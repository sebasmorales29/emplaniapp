using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.Abstracciones.InterfacesParaUI;
using Emplaniapp.LogicaDeNegocio;
using Emplaniapp.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.IO;

namespace Emplaniapp.UI.Controllers
{
    [Authorize]
    public class DatosPersonalesController : Controller
    {
        private IDatosPersonalesLN _datosPersonalesLN;
        private IObservacionLN _observacionLN;
        private ApplicationUserManager _userManager;

        // Constructor para uso normal del controlador
        public DatosPersonalesController()
        {
            _datosPersonalesLN = new DatosPersonalesLN();
            _observacionLN = new ObservacionLN();
        }

        // Constructor para inyección de dependencias (usado por Identity/OWIN)
        public DatosPersonalesController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
            _datosPersonalesLN = new DatosPersonalesLN();
            _observacionLN = new ObservacionLN();
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }



        // ACCIÓN UNIFICADA PARA MOSTRAR EL PERFIL DEL EMPLEADO Y SUS SECCIONES
        public ActionResult Detalles(int? id, string seccion = "Datos personales")
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

            ViewBag.Seccion = seccion;
            return View(empleado);
        }

        


        // GET: DatosPersonales/EditarDatosPersonales/5
        public ActionResult EditarDatosPersonales(int id)
        {
            var empleado = _datosPersonalesLN.ObtenerEmpleadoPorId(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }

        // POST: DatosPersonales/ActualizarDatosPersonales
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActualizarDatosPersonales(EmpleadoDto model)
        {
            if (ModelState.IsValid)
            {
                bool resultado = _datosPersonalesLN.ActualizarDatosPersonales(model);
                if (resultado)
                {
                    TempData["Mensaje"] = "Datos personales actualizados con éxito.";
                    return RedirectToAction("Detalles", new { id = model.idEmpleado });
                }
                else
                {
                    ModelState.AddModelError("", "No se pudieron actualizar los datos.");
                }
            }
            return View("EditarDatosPersonales", model);
        }



        // GET: DatosPersonales/EditarDatosLaborales/5
        public ActionResult EditarDatosLaborales(int id)
        {
            var empleado = _datosPersonalesLN.ObtenerEmpleadoPorId(id);
            
            if (empleado == null)
            {
                return HttpNotFound();
            }
            
            var datosLaborales = new DatosLaboralesViewModel
            {
                IdEmpleado = empleado.idEmpleado,
                NumeroOcupacion = "9832", // Por ahora hardcodeado
                IdCargo = empleado.idCargo,
                Cargo = empleado.nombreCargo,
                FechaIngreso = empleado.fechaContratacion,
                FechaSalida = empleado.fechaSalida,
                InicioVacaciones = null // Por ahora null
            };
            
            ViewBag.Cargos = ObtenerCargosSelectList(empleado.idCargo);
            
            return View(datosLaborales);
        }


        // POST: DatosPersonales/EditarDatosLaborales/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarDatosLaborales(DatosLaboralesViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool resultado = _datosPersonalesLN.ActualizarDatosLaborales(
                    model.IdEmpleado, 
                    (int)model.IdCargo, 
                    model.FechaIngreso, 
                    model.FechaSalida);

                if (resultado)
                {
                    TempData["Mensaje"] = "Datos laborales actualizados correctamente";
                    TempData["TipoMensaje"] = "success";
                    return RedirectToAction("Detalles", new { id = model.IdEmpleado });
                }
                else
                {
                    ModelState.AddModelError("", "Error al guardar los cambios");
                }
            }
            
            ViewBag.Cargos = ObtenerCargosSelectList(model.IdCargo);
            return View(model);
        }

        // GET: DatosPersonales/EditarDatosFinancieros/5
        public ActionResult EditarDatosFinancieros(int id)
        {
            var empleado = _datosPersonalesLN.ObtenerEmpleadoPorId(id);
            
            if (empleado == null)
            {
                return HttpNotFound();
            }
            
            var datosFinancieros = new DatosFinancierosViewModel
            {
                IdEmpleado = empleado.idEmpleado,
                PeriocidadPago = empleado.periocidadPago,
                SalarioAprobado = empleado.salarioAprobado,
                SalarioDiario = empleado.salarioDiario,
                IdTipoMoneda = empleado.idMoneda,
                TipoMoneda = empleado.nombreMoneda,
                CuentaIBAN = empleado.cuentaIBAN,
                IdBanco = empleado.idBanco,
                Banco = empleado.nombreBanco
            };
            
            ViewBag.TiposMoneda = ObtenerTiposMonedasSelectList(empleado.idMoneda);
            ViewBag.Bancos = ObtenerBancosSelectList(empleado.idBanco);
            ViewBag.PeriocidadesPago = ObtenerPeriocidadesPagoSelectList(empleado.periocidadPago);
            
            return View(datosFinancieros);
        }


        // POST: DatosPersonales/EditarDatosFinancieros/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarDatosFinancieros(DatosFinancierosViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool resultado = _datosPersonalesLN.ActualizarDatosFinancieros(
                    model.IdEmpleado,
                    model.SalarioAprobado,
                    model.SalarioDiario,
                    model.PeriocidadPago,
                    (int)model.IdTipoMoneda,
                    model.CuentaIBAN,
                    (int)model.IdBanco);

                if (resultado)
                {
                    TempData["Mensaje"] = "Datos financieros actualizados correctamente";
                    TempData["TipoMensaje"] = "success";
                    return RedirectToAction("Detalles", new { id = model.IdEmpleado });
                }
                else
                {
                    ModelState.AddModelError("", "Error al guardar los cambios");
                }
            }
            
            ViewBag.TiposMoneda = ObtenerTiposMonedasSelectList(model.IdTipoMoneda);
            ViewBag.Bancos = ObtenerBancosSelectList(model.IdBanco);
            ViewBag.PeriocidadesPago = ObtenerPeriocidadesPagoSelectList(model.PeriocidadPago);
            return View(model);
        }



        #region Métodos Auxiliares
        
        private SelectList ObtenerCargosSelectList(object selectedValue = null)
        {
            var cargos = _datosPersonalesLN.ObtenerCargos()
                .Select(c => new SelectListItem
                {
                    Value = c.idCargo.ToString(),
                    Text = c.nombreCargo
                }).ToList();
            
            return new SelectList(cargos, "Value", "Text", selectedValue);
        }

        private SelectList ObtenerTiposMonedasSelectList(object selectedValue = null)
        {
            var monedas = _datosPersonalesLN.ObtenerTiposMoneda()
                .Select(m => new SelectListItem
                {
                    Value = m.idMoneda.ToString(),
                    Text = m.nombreMoneda
                }).ToList();
            
            return new SelectList(monedas, "Value", "Text", selectedValue);
        }

        private SelectList ObtenerBancosSelectList(object selectedValue = null)
        {
            var bancos = _datosPersonalesLN.ObtenerBancos()
                .Select(b => new SelectListItem
                {
                    Value = b.idBanco.ToString(),
                    Text = b.nombreBanco
                }).ToList();
            
            return new SelectList(bancos, "Value", "Text", selectedValue);
        }

        private SelectList ObtenerPeriocidadesPagoSelectList(object selectedValue = null)
        {
            var periodicidades = new List<object>
            {
                new { Value = "Quincenal", Text = "Quincenal" },
                new { Value = "Mensual", Text = "Mensual" }
            };
            return new SelectList(periodicidades, "Value", "Text", selectedValue);
        }

        #endregion



        #region Observaciones

        // GET: DatosPersonales/ObtenerListaObservaciones/5
        [HttpGet]
        public ActionResult ObtenerListaObservaciones(int id, int? ano, int? mes, string usuarioId)
        {
            // 1. Obtener TODAS las observaciones para este empleado para popular los filtros
            var todasLasObservaciones = _observacionLN.ObtenerObservacionesPorEmpleado(id);

            // 2. Llenar los ViewBags para los dropdowns de filtros
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

            // Guardar los filtros seleccionados para devolverlos a la vista
            ViewBag.AnoSeleccionado = ano;
            ViewBag.MesSeleccionado = mes;
            ViewBag.UsuarioSeleccionado = usuarioId;
            ViewBag.FiltrosActivos = ano.HasValue || mes.HasValue || !string.IsNullOrEmpty(usuarioId);


            // 3. Filtrar la lista de observaciones según los parámetros recibidos
            var observacionesFiltradas = todasLasObservaciones;

            if (ano.HasValue)
            {
                observacionesFiltradas = observacionesFiltradas.Where(o => o.FechaCreacion.Year == ano.Value).ToList();
            }
            if (mes.HasValue)
            {
                observacionesFiltradas = observacionesFiltradas.Where(o => o.FechaCreacion.Month == mes.Value).ToList();
            }
            if (!string.IsNullOrEmpty(usuarioId))
            {
                observacionesFiltradas = observacionesFiltradas.Where(o => o.IdUsuarioCreo == usuarioId || o.IdUsuarioEdito == usuarioId).ToList();
            }

            // 4. Poblar los nombres de usuario para la lista filtrada
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


            // 5. Renderizar y devolver el resultado
            ViewBag.IdEmpleado = id;
            string partialViewHtml = RenderRazorViewToString("_ObservacionesList", observacionesFiltradas);
            return Json(new { success = true, html = partialViewHtml }, JsonRequestBehavior.AllowGet);
        }


        // GET: DatosPersonales/AgregarObservacion
        [HttpGet]
        public ActionResult AgregarObservacion(int idEmpleado)
        {
            var model = new ObservacionDto
            {
                IdEmpleado = idEmpleado
            };
            return PartialView("_ObservacionForm", model);
        }

        // POST: DatosPersonales/AgregarObservacion
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
                    return Json(new { success = true });
                }
                ModelState.AddModelError("", "Ocurrió un error al guardar la observación.");
            }
            return PartialView("_ObservacionForm", model);
        }

        // GET: DatosPersonales/EditarObservacion/5
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

        // POST: DatosPersonales/EditarObservacion/5
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
        #endregion
    }
}