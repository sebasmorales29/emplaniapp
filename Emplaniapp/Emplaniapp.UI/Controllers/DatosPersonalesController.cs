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
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.Entidades;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using Emplaniapp.AccesoADatos;

namespace Emplaniapp.UI.Controllers
{
    [Authorize]
    public class DatosPersonalesController : Controller
    {
        private IDatosPersonalesLN _datosPersonalesLN;
        private IObservacionLN _observacionLN;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        // Constructor para uso normal del controlador
        public DatosPersonalesController()
        {
            _datosPersonalesLN = new DatosPersonalesLN();
            _observacionLN = new ObservacionLN();
        }

        // Constructor para inyección de dependencias (usado por Identity/OWIN)
        public DatosPersonalesController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            _datosPersonalesLN = new DatosPersonalesLN();
            _observacionLN = new ObservacionLN();
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        public ApplicationRoleManager RoleManager
        {
            get => _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            private set => _roleManager = value;
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

            // 🔥 CARGAR DATOS GEOGRÁFICOS PARA DROPDOWNS
            ViewBag.Provincias = ObtenerProvinciasSelectList(empleado.idProvincia);
            ViewBag.Cantones = ObtenerCantonesSelectList(empleado.idProvincia, empleado.idCanton);
            ViewBag.Distritos = ObtenerDistritosSelectList(empleado.idCanton, empleado.idDistrito);
            ViewBag.Calles = ObtenerCallesSelectList(empleado.idDistrito, empleado.idCalle);

            return View(empleado);
        }

        // POST: DatosPersonales/ActualizarDatosPersonales
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActualizarDatosPersonales(EmpleadoDto model)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"=== ACTUALIZACION DATOS PERSONALES ===");
                System.Diagnostics.Debug.WriteLine($"Empleado ID: {model.idEmpleado}");
                System.Diagnostics.Debug.WriteLine($"Nombre: {model.nombre}");
                System.Diagnostics.Debug.WriteLine($"Primer Apellido: {model.primerApellido}");
                System.Diagnostics.Debug.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");
                
                if (!ModelState.IsValid)
                {
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        System.Diagnostics.Debug.WriteLine($"Error de validación: {error.ErrorMessage}");
                    }
                }

                if (ModelState.IsValid)
                {
                    System.Diagnostics.Debug.WriteLine($"Llamando a _datosPersonalesLN.ActualizarDatosPersonales");
                    bool resultado = _datosPersonalesLN.ActualizarDatosPersonales(model);
                    System.Diagnostics.Debug.WriteLine($"Resultado de ActualizarDatosPersonales: {resultado}");
                    
                    if (resultado)
                    {
                        TempData["Mensaje"] = "Datos personales actualizados con éxito.";
                        TempData["TipoMensaje"] = "success";
                        return RedirectToAction("Detalles", new { id = model.idEmpleado });
                    }
                    else
                    {
                        ModelState.AddModelError("", "No se pudieron actualizar los datos.");
                        System.Diagnostics.Debug.WriteLine("Error: No se pudieron actualizar los datos personales");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Excepción en ActualizarDatosPersonales: {ex.Message}");
                ModelState.AddModelError("", "Ocurrió un error inesperado: " + ex.Message);
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
                FechaSalida = empleado.fechaSalida
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
            
            System.Diagnostics.Debug.WriteLine($"=== CARGAR DATOS FINANCIEROS GET ===");
            System.Diagnostics.Debug.WriteLine($"IdEmpleado: {empleado.idEmpleado}");
            System.Diagnostics.Debug.WriteLine($"SalarioAprobado desde BD: {empleado.salarioAprobado}");
            System.Diagnostics.Debug.WriteLine($"SalarioDiario desde BD: {empleado.salarioDiario}");
            System.Diagnostics.Debug.WriteLine($"PeriocidadPago: {empleado.periocidadPago}");
            
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
            
            System.Diagnostics.Debug.WriteLine($"SalarioAprobado en ViewModel: {datosFinancieros.SalarioAprobado}");
            
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
            try
            {
                System.Diagnostics.Debug.WriteLine($"=== ACTUALIZACIÓN DATOS FINANCIEROS ===");
                System.Diagnostics.Debug.WriteLine($"IdEmpleado: {model.IdEmpleado}");
                System.Diagnostics.Debug.WriteLine($"SalarioAprobado: {model.SalarioAprobado} (pre-cargado y editable)");
                System.Diagnostics.Debug.WriteLine($"PeriocidadPago: {model.PeriocidadPago}");
                System.Diagnostics.Debug.WriteLine($"IdTipoMoneda: {model.IdTipoMoneda}");
                System.Diagnostics.Debug.WriteLine($"CuentaIBAN: {model.CuentaIBAN}");
                System.Diagnostics.Debug.WriteLine($"IdBanco: {model.IdBanco}");
                System.Diagnostics.Debug.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

                if (!ModelState.IsValid)
                {
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        System.Diagnostics.Debug.WriteLine($"Error de validación: {error.ErrorMessage}");
                    }
                }

                if (ModelState.IsValid)
                {
                    // Obtener los datos originales del empleado para mantener salarioDiario
                    var empleadoOriginal = _datosPersonalesLN.ObtenerEmpleadoPorId(model.IdEmpleado);
                    
                    if (empleadoOriginal == null)
                    {
                        System.Diagnostics.Debug.WriteLine("ERROR: No se encontró el empleado");
                        ModelState.AddModelError("", "No se encontró el empleado.");
                        ViewBag.TiposMoneda = ObtenerTiposMonedasSelectList(model.IdTipoMoneda);
                        ViewBag.Bancos = ObtenerBancosSelectList(model.IdBanco);
                        ViewBag.PeriocidadesPago = ObtenerPeriocidadesPagoSelectList(model.PeriocidadPago);
                        return View(model);
                    }

                    System.Diagnostics.Debug.WriteLine($"SalarioAprobado original: {empleadoOriginal.salarioAprobado}, usando del formulario: {model.SalarioAprobado}");

                    // Actualizar todos los campos editables: salarioAprobado, periodicidad, moneda, IBAN y banco
                    // SalarioAprobado viene pre-cargado pero se puede modificar
                    bool resultado = _datosPersonalesLN.ActualizarDatosFinancieros(
                        model.IdEmpleado,
                        model.SalarioAprobado,           // Usar valor del formulario (editable)
                        empleadoOriginal.salarioDiario,  // Mantener valor original (calculado)
                        model.PeriocidadPago,
                        (int)model.IdTipoMoneda,
                        model.CuentaIBAN,
                        (int)model.IdBanco);

                    System.Diagnostics.Debug.WriteLine($"Resultado de ActualizarDatosFinancieros: {resultado}");

                    if (resultado)
                    {
                        TempData["Mensaje"] = "Datos financieros actualizados correctamente";
                        TempData["TipoMensaje"] = "success";
                        System.Diagnostics.Debug.WriteLine("=== ACTUALIZACIÓN EXITOSA ===");
                        return RedirectToAction("Detalles", new { id = model.IdEmpleado });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error al guardar los cambios");
                        System.Diagnostics.Debug.WriteLine("ERROR: Falló ActualizarDatosFinancieros");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPCIÓN en EditarDatosFinancieros: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                ModelState.AddModelError("", "Ocurrió un error inesperado: " + ex.Message);
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

        #region Gestión de Roles y Permisos

        // GET: Obtener roles del empleado (solo Administradores)
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<JsonResult> ObtenerRolesEmpleado(int idEmpleado)
        {
            try
            {
                var empleado = _datosPersonalesLN.ObtenerEmpleadoPorId(idEmpleado);
                if (empleado == null)
                {
                    return Json(new { success = false, message = "Empleado no encontrado." }, JsonRequestBehavior.AllowGet);
                }

                // Definir los 3 roles del sistema
                var todosLosRoles = new[] { "Administrador", "Contador", "Empleado" };
                var rolesAsignados = new List<string>();

                // Buscar usuario por IdNetUser o por email
                ApplicationUser user = null;
                if (!string.IsNullOrEmpty(empleado.IdNetUser))
                {
                    user = await UserManager.FindByIdAsync(empleado.IdNetUser);
                }
                
                if (user == null && !string.IsNullOrEmpty(empleado.correoInstitucional))
                {
                    user = await UserManager.FindByEmailAsync(empleado.correoInstitucional);
                }

                if (user != null)
                {
                    rolesAsignados = (await UserManager.GetRolesAsync(user.Id)).ToList();
                }

                // Crear la respuesta con todos los roles
                var rolesData = todosLosRoles.Select(rol => new
                {
                    nombre = rol,
                    asignado = rolesAsignados.Contains(rol)
                }).ToList();

                return Json(new { 
                    success = true, 
                    roles = rolesData
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al obtener roles: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Asignar rol al empleado (solo Administradores)
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AsignarRol(int idEmpleado, string nombreRol)
        {
            try
            {
                var empleado = _datosPersonalesLN.ObtenerEmpleadoPorId(idEmpleado);
                if (empleado == null)
                {
                    return Json(new { success = false, message = "Empleado no encontrado." });
                }

                // Buscar usuario por IdNetUser o por email
                ApplicationUser user = null;
                if (!string.IsNullOrEmpty(empleado.IdNetUser))
                {
                    user = await UserManager.FindByIdAsync(empleado.IdNetUser);
                }
                else
                {
                    user = await UserManager.FindByEmailAsync(empleado.correoInstitucional);
                }

                if (user == null)
                {
                    return Json(new { success = false, message = "No se encontró el usuario asociado a este empleado." });
                }

                // Verificar que el rol existe
                if (!await RoleManager.RoleExistsAsync(nombreRol))
                {
                    return Json(new { success = false, message = "El rol especificado no existe." });
                }

                // Verificar si ya tiene el rol
                if (await UserManager.IsInRoleAsync(user.Id, nombreRol))
                {
                    return Json(new { success = false, message = "El usuario ya tiene este rol asignado." });
                }

                var result = await UserManager.AddToRoleAsync(user.Id, nombreRol);
                if (result.Succeeded)
                {
                    // Invalidar la sesión del usuario afectado para forzar actualización de roles
                    await InvalidateUserSessions(user.Id);
                    
                    return Json(new { success = true, message = $"Rol '{nombreRol}' asignado correctamente a {empleado.nombre} {empleado.primerApellido}.", requiresRefresh = true });
                }
                else
                {
                    return Json(new { success = false, message = "Error al asignar el rol: " + string.Join(", ", result.Errors) });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al asignar rol: " + ex.Message });
            }
        }

        // POST: Remover rol del empleado (solo Administradores)
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> RemoverRol(int idEmpleado, string nombreRol)
        {
            try
            {
                var empleado = _datosPersonalesLN.ObtenerEmpleadoPorId(idEmpleado);
                if (empleado == null)
                {
                    return Json(new { success = false, message = "Empleado no encontrado." });
                }

                // Buscar usuario por IdNetUser o por email
                ApplicationUser user = null;
                if (!string.IsNullOrEmpty(empleado.IdNetUser))
                {
                    user = await UserManager.FindByIdAsync(empleado.IdNetUser);
                }
                else
                {
                    user = await UserManager.FindByEmailAsync(empleado.correoInstitucional);
                }

                if (user == null)
                {
                    return Json(new { success = false, message = "No se encontró el usuario asociado a este empleado." });
                }

                // Verificar si tiene el rol
                if (!await UserManager.IsInRoleAsync(user.Id, nombreRol))
                {
                    return Json(new { success = false, message = "El usuario no tiene este rol asignado." });
                }

                var result = await UserManager.RemoveFromRoleAsync(user.Id, nombreRol);
                if (result.Succeeded)
                {
                    // Invalidar la sesión del usuario afectado para forzar actualización de roles
                    await InvalidateUserSessions(user.Id);
                    
                    return Json(new { success = true, message = $"Rol '{nombreRol}' removido correctamente de {empleado.nombre} {empleado.primerApellido}.", requiresRefresh = true });
                }
                else
                {
                    return Json(new { success = false, message = "Error al remover el rol: " + string.Join(", ", result.Errors) });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al remover rol: " + ex.Message });
            }
        }

        // POST: Validar contraseña de administrador
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ValidateAdminPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return Json(new { success = false, message = "La contraseña no puede estar vacía." });
            }

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return Json(new { success = false, message = "No se pudo identificar al usuario." });
            }

            var correctPassword = await UserManager.CheckPasswordAsync(user, password);

            if (correctPassword)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Contraseña incorrecta." });
            }
        }

        /// <summary>
        /// Obtiene el rol de mayor prioridad de una lista de roles
        /// </summary>
        /// <param name="roles">Lista de roles del usuario</param>
        /// <returns>Rol de mayor prioridad</returns>
        private string GetHighestPriorityRole(List<string> roles)
        {
            // Jerarquía de roles: Administrador > Contador > Empleado
            var rolesPriority = new List<string> { "Administrador", "Contador", "Empleado" };
            
            foreach (var priorityRole in rolesPriority)
            {
                if (roles.Contains(priorityRole))
                {
                    System.Diagnostics.Debug.WriteLine($"Rol de mayor prioridad seleccionado: {priorityRole}");
                    return priorityRole;
                }
            }
            
            // Fallback por si no encuentra ningún rol conocido
            var fallbackRole = roles.FirstOrDefault() ?? "Empleado";
            System.Diagnostics.Debug.WriteLine($"Usando rol fallback: {fallbackRole}");
            return fallbackRole;
        }

        /// <summary>
        /// Invalida las sesiones activas de un usuario específico
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        private async Task InvalidateUserSessions(string userId)
        {
            try
            {
                // Actualizar el SecurityStamp del usuario para invalidar todas sus sesiones activas
                var user = await UserManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await UserManager.UpdateSecurityStampAsync(userId);
                    System.Diagnostics.Debug.WriteLine($"SecurityStamp actualizado para usuario {userId}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al invalidar sesiones del usuario {userId}: {ex.Message}");
            }
        }

        /// <summary>
        /// Verifica si los roles del usuario actual han cambiado desde su última verificación
        /// </summary>
        /// <returns>JSON indicando si hay cambios en los roles</returns>
        [HttpGet]
        public async Task<JsonResult> VerificarCambiosRoles()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Usuario no encontrado." }, JsonRequestBehavior.AllowGet);
                }

                // Obtener roles actuales de la base de datos
                var rolesActuales = await UserManager.GetRolesAsync(userId);
                var rolesActualesList = rolesActuales.ToList();

                // Obtener roles almacenados en la sesión (si existen)
                var rolesEnSesion = Session["UserRoles"] as List<string> ?? new List<string>();

                // Comparar roles
                bool hayDiferencias = !rolesActualesList.OrderBy(r => r).SequenceEqual(rolesEnSesion.OrderBy(r => r));

                if (hayDiferencias)
                {
                    // Actualizar roles en sesión
                    Session["UserRoles"] = rolesActualesList;
                    
                    // Verificar si el rol activo sigue siendo válido
                    var rolActivo = Session["ActiveRole"] as string;
                    if (!string.IsNullOrEmpty(rolActivo) && !rolesActualesList.Contains(rolActivo))
                    {
                        // El rol activo ya no es válido, cambiar al rol de mayor prioridad disponible
                        Session["ActiveRole"] = GetHighestPriorityRole(rolesActualesList);
                    }

                    return Json(new { 
                        success = true, 
                        hasChanges = true, 
                        newRoles = rolesActualesList,
                        activeRole = Session["ActiveRole"] as string
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { success = true, hasChanges = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al verificar cambios: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Role Switcher Methods

        /// <summary>
        /// Obtiene información sobre los roles del usuario actual
        /// </summary>
        /// <returns>Información de roles en formato JSON</returns>
        [HttpGet]
        public async Task<JsonResult> GetUserRoleInfo()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"=== GetUserRoleInfo called ===");
                System.Diagnostics.Debug.WriteLine($"User authenticated: {User.Identity.IsAuthenticated}");
                System.Diagnostics.Debug.WriteLine($"User name: {User.Identity.Name}");
                
                var userId = User.Identity.GetUserId();
                var userRoles = await UserManager.GetRolesAsync(userId);
                var rolesList = userRoles.ToList();
                
                // Inicializar roles en sesión para comparación futura
                Session["UserRoles"] = rolesList;
                
                System.Diagnostics.Debug.WriteLine($"User roles: {string.Join(", ", rolesList)}");
                
                // Obtener rol activo de la sesión
                var activeRole = Session["ActiveRole"] as string;
                if (string.IsNullOrEmpty(activeRole) || !rolesList.Contains(activeRole))
                {
                    // Establecer rol activo según prioridad: Administrador > Contador > Empleado
                    activeRole = GetHighestPriorityRole(rolesList);
                    Session["ActiveRole"] = activeRole;
                }
                
                System.Diagnostics.Debug.WriteLine($"Active role: {activeRole}");
                
                var hasMultipleRoles = rolesList.Count > 1;
                System.Diagnostics.Debug.WriteLine($"Has multiple roles: {hasMultipleRoles}");

                var result = new
                {
                    success = true,
                    activeRole = activeRole,
                    availableRoles = rolesList,
                    hasMultipleRoles = hasMultipleRoles,
                    debug = new
                    {
                        userAuthenticated = User.Identity.IsAuthenticated,
                        userName = User.Identity.Name,
                        rolesCount = rolesList.Count
                    }
                };

                System.Diagnostics.Debug.WriteLine($"Returning JSON: {Newtonsoft.Json.JsonConvert.SerializeObject(result)}");
                
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in GetUserRoleInfo: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                
                return Json(new
                {
                    success = false,
                    message = "Error al obtener información de roles: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Cambia el rol activo del usuario
        /// </summary>
        /// <param name="role">Nuevo rol a activar</param>
        /// <returns>Resultado JSON del cambio</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SwitchRole(string role)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"SwitchRole called with role: {role}");
                
                if (string.IsNullOrEmpty(role))
                {
                    return Json(new { success = false, message = "El rol no puede estar vacío." });
                }

                var userId = User.Identity.GetUserId();
                var userRoles = await UserManager.GetRolesAsync(userId);
                var rolesList = userRoles.ToList();

                System.Diagnostics.Debug.WriteLine($"User has roles: {string.Join(", ", rolesList)}");
                
                if (rolesList.Contains(role))
                {
                    Session["ActiveRole"] = role;
                    System.Diagnostics.Debug.WriteLine($"Role switched successfully to: {role}");
                    
                    return Json(new { 
                        success = true, 
                        message = $"Rol cambiado a '{role}' exitosamente.",
                        newRole = role
                    });
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"User does not have role: {role}");
                    return Json(new { 
                        success = false, 
                        message = "No tienes permisos para acceder a este rol." 
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in SwitchRole: {ex.Message}");
                return Json(new { 
                    success = false, 
                    message = "Error al cambiar el rol: " + ex.Message 
                });
            }
        }

        /// <summary>
        /// Fuerza la actualización del rol activo según la prioridad de roles
        /// </summary>
        /// <returns>JSON con el rol activo actualizado</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> RefreshActiveRole()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Usuario no encontrado." });
                }

                var userRoles = await UserManager.GetRolesAsync(userId);
                var rolesList = userRoles.ToList();

                // Establecer el rol de mayor prioridad
                var newActiveRole = GetHighestPriorityRole(rolesList);
                Session["ActiveRole"] = newActiveRole;
                Session["UserRoles"] = rolesList;

                System.Diagnostics.Debug.WriteLine($"🔄 Rol activo actualizado manualmente a: {newActiveRole}");

                return Json(new { 
                    success = true, 
                    message = $"Rol activo actualizado a '{newActiveRole}'",
                    activeRole = newActiveRole,
                    availableRoles = rolesList
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al actualizar rol activo: " + ex.Message });
            }
        }

        #endregion

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

        // ===============================================
        // 🔥 MÉTODOS AUXILIARES PARA DATOS GEOGRÁFICOS
        // ===============================================
        
        private SelectList ObtenerProvinciasSelectList(int? selectedValue = null)
        {
            using (var contexto = new Contexto())
            {
                var provincias = contexto.Provincia
                    .Select(p => new { p.idProvincia, p.nombreProvincia })
                    .OrderBy(p => p.nombreProvincia)
                    .ToList();
                
                return new SelectList(provincias, "idProvincia", "nombreProvincia", selectedValue);
            }
        }

        private SelectList ObtenerCantonesSelectList(int? idProvincia = null, int? selectedValue = null)
        {
            using (var contexto = new Contexto())
            {
                var query = contexto.Canton.AsQueryable();
                
                if (idProvincia.HasValue)
                {
                    query = query.Where(c => c.idProvincia == idProvincia.Value);
                }
                
                var cantones = query
                    .Select(c => new { c.idCanton, c.nombreCanton, c.idProvincia })
                    .OrderBy(c => c.nombreCanton)
                    .ToList();
                
                return new SelectList(cantones, "idCanton", "nombreCanton", selectedValue);
            }
        }

        private SelectList ObtenerDistritosSelectList(int? idCanton = null, int? selectedValue = null)
        {
            using (var contexto = new Contexto())
            {
                var query = contexto.Distrito.AsQueryable();
                
                if (idCanton.HasValue)
                {
                    query = query.Where(d => d.idCanton == idCanton.Value);
                }
                
                var distritos = query
                    .Select(d => new { d.idDistrito, d.nombreDistrito, d.idCanton })
                    .OrderBy(d => d.nombreDistrito)
                    .ToList();
                
                return new SelectList(distritos, "idDistrito", "nombreDistrito", selectedValue);
            }
        }

        private SelectList ObtenerCallesSelectList(int? idDistrito = null, int? selectedValue = null)
        {
            using (var contexto = new Contexto())
            {
                var query = contexto.Calle.AsQueryable();
                
                if (idDistrito.HasValue)
                {
                    query = query.Where(c => c.idDistrito == idDistrito.Value);
                }
                
                var calles = query
                    .Select(c => new { c.idCalle, c.nombreCalle, c.idDistrito })
                    .OrderBy(c => c.nombreCalle)
                    .ToList();
                
                return new SelectList(calles, "idCalle", "nombreCalle", selectedValue);
            }
        }

        // ===============================================
        // MÉTODOS AJAX PARA DROPDOWNS EN CASCADA
        // ===============================================
        
        [HttpGet]
        public JsonResult ObtenerCantonesPorProvincia(int idProvincia)
        {
            try
            {
                using (var contexto = new Contexto())
                {
                    var cantones = contexto.Canton
                        .Where(c => c.idProvincia == idProvincia)
                        .Select(c => new { value = c.idCanton, text = c.nombreCanton })
                        .OrderBy(c => c.text)
                        .ToList();

                    System.Diagnostics.Debug.WriteLine($"Cargando cantones para provincia {idProvincia}: {cantones.Count} cantones encontrados");
                    return Json(cantones, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error obteniendo cantones: {ex.Message}");
                return Json(new List<object>(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ObtenerDistritosPorCanton(int idCanton)
        {
            try
            {
                using (var contexto = new Contexto())
                {
                    var distritos = contexto.Distrito
                        .Where(d => d.idCanton == idCanton)
                        .Select(d => new { value = d.idDistrito, text = d.nombreDistrito })
                        .OrderBy(d => d.text)
                        .ToList();

                    System.Diagnostics.Debug.WriteLine($"Cargando distritos para cantón {idCanton}: {distritos.Count} distritos encontrados");
                    return Json(distritos, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error obteniendo distritos: {ex.Message}");
                return Json(new List<object>(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ObtenerCallesPorDistrito(int idDistrito)
        {
            try
            {
                using (var contexto = new Contexto())
                {
                    var calles = contexto.Calle
                        .Where(c => c.idDistrito == idDistrito)
                        .Select(c => new { value = c.idCalle, text = c.nombreCalle })
                        .OrderBy(c => c.text)
                        .ToList();

                    System.Diagnostics.Debug.WriteLine($"Cargando calles para distrito {idDistrito}: {calles.Count} calles encontradas");
                    return Json(calles, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error obteniendo calles: {ex.Message}");
                return Json(new List<object>(), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}