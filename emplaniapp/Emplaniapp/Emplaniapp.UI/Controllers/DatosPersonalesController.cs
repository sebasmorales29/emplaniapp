using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.Abstracciones.InterfacesParaUI;
using Emplaniapp.Abstracciones.InterfacesParaUI.Historial;
using Emplaniapp.LogicaDeNegocio;
using Emplaniapp.LogicaDeNegocio.Historial;
using Emplaniapp.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.IO;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.Entidades;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using Emplaniapp.AccesoADatos;
using Emplaniapp.UI.Attributes;

namespace Emplaniapp.UI.Controllers
{
    [Authorize]
    public class DatosPersonalesController : Controller
    {
        private IDatosPersonalesLN _datosPersonalesLN;
        private IObservacionLN _observacionLN;
        private IRegistrarEventoHistorialLN _registrarEventoHistorialLN;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        // Constructor para uso normal del controlador
        public DatosPersonalesController()
        {
            _datosPersonalesLN = new DatosPersonalesLN();
            _observacionLN = new ObservacionLN();
            _registrarEventoHistorialLN = new RegistrarEventoHistorialLN();
        }

        // Constructor para inyección de dependencias (usado por Identity/OWIN)
        public DatosPersonalesController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            _datosPersonalesLN = new DatosPersonalesLN();
            _observacionLN = new ObservacionLN();
            _registrarEventoHistorialLN = new RegistrarEventoHistorialLN();
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
            // 1) Determinar rol activo (Session si existe; si no, por prioridad de claims)
            var activeRole = Session["ActiveRole"] as string;
            if (string.IsNullOrWhiteSpace(activeRole))
            {
                if (User.IsInRole("Administrador")) activeRole = "Administrador";
                else if (User.IsInRole("Contador")) activeRole = "Contador";
                else activeRole = "Empleado";
            }

            // 2) Resolver id desde todas las fuentes (parámetro, ruta, querystring)
            int resolvedId = 0;

            if (id.HasValue && id.Value > 0)
            {
                resolvedId = id.Value;
            }
            else
            {
                object routeIdObj;
                if (ControllerContext != null &&
                    ControllerContext.RouteData != null &&
                    ControllerContext.RouteData.Values.TryGetValue("id", out routeIdObj))
                {
                    int tmp;
                    if (routeIdObj != null && int.TryParse(routeIdObj.ToString(), out tmp) && tmp > 0)
                        resolvedId = tmp;
                }

                if (resolvedId == 0)
                {
                    int tmp;
                    var qsId = Request["id"];
                    if (!string.IsNullOrWhiteSpace(qsId) && int.TryParse(qsId, out tmp) && tmp > 0)
                        resolvedId = tmp;
                }

                if (resolvedId == 0)
                {
                    int tmp;
                    var qsIdEmp = Request["idEmpleado"];
                    if (!string.IsNullOrWhiteSpace(qsIdEmp) && int.TryParse(qsIdEmp, out tmp) && tmp > 0)
                        resolvedId = tmp;
                }
            }

            // 3) Si el rol activo es Empleado -> siempre su propio id (ignora cualquier id recibido)
            if (activeRole.Equals("Empleado", StringComparison.OrdinalIgnoreCase))
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var idEmpleadoClaim = claimsIdentity?.FindFirst("idEmpleado");
                int claimId;
                if (idEmpleadoClaim == null || !int.TryParse(idEmpleadoClaim.Value, out claimId))
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "No se pudo identificar al empleado.");
                }
                resolvedId = claimId;
            }
            else
            {
                // 4) Admin/Contador: si no se resolvió id por URL/ruta/query, usar claim como fallback
                if (resolvedId == 0)
                {
                    var claimsIdentity = User.Identity as ClaimsIdentity;
                    var idEmpleadoClaim = claimsIdentity?.FindFirst("idEmpleado");
                    int claimId;
                    if (idEmpleadoClaim == null || !int.TryParse(idEmpleadoClaim.Value, out claimId))
                    {
                        return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "No se pudo identificar al empleado.");
                    }
                    resolvedId = claimId;
                }
            }

            // 5) Cargar el empleado y devolver vista
            var empleado = _datosPersonalesLN.ObtenerEmpleadoPorId(resolvedId);
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

            // 🔥 CARGAR DATOS GEOGRÁFICOS PARA DROPDOWN
            ViewBag.Provincias = ObtenerProvinciasSelectList(empleado.idProvincia);

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
                    // Obtener datos anteriores para comparar
                    var empleadoAnterior = _datosPersonalesLN.ObtenerEmpleadoPorId(model.idEmpleado);

                    System.Diagnostics.Debug.WriteLine($"Llamando a _datosPersonalesLN.ActualizarDatosPersonales");
                    bool resultado = _datosPersonalesLN.ActualizarDatosPersonales(model);
                    System.Diagnostics.Debug.WriteLine($"Resultado de ActualizarDatosPersonales: {resultado}");

                    if (resultado)
                    {
                        // Registrar evento en el historial
                        try
                        {
                            var cambios = new List<string>();

                            if (empleadoAnterior.nombre != model.nombre)
                                cambios.Add($"Nombre: {empleadoAnterior.nombre} → {model.nombre}");

                            if (empleadoAnterior.primerApellido != model.primerApellido)
                                cambios.Add($"Primer Apellido: {empleadoAnterior.primerApellido} → {model.primerApellido}");

                            if (empleadoAnterior.segundoApellido != model.segundoApellido)
                                cambios.Add($"Segundo Apellido: {empleadoAnterior.segundoApellido} → {model.segundoApellido}");

                            if (empleadoAnterior.fechaNacimiento != model.fechaNacimiento)
                                cambios.Add($"Fecha Nacimiento: {empleadoAnterior.fechaNacimiento:dd/MM/yyyy} → {model.fechaNacimiento:dd/MM/yyyy}");

                            if (empleadoAnterior.direccionDetallada != model.direccionDetallada)
                                cambios.Add($"Dirección Detallada: {empleadoAnterior.direccionDetallada} → {model.direccionDetallada}");

                            if (empleadoAnterior.nombreProvincia != model.nombreProvincia)
                                cambios.Add($"Provincia: {empleadoAnterior.nombreProvincia} → {model.nombreProvincia}");

                            if (empleadoAnterior.nombreCanton != model.nombreCanton)
                                cambios.Add($"Cantón: {empleadoAnterior.nombreCanton} → {model.nombreCanton}");

                            if (empleadoAnterior.nombreDistrito != model.nombreDistrito)
                                cambios.Add($"Distrito: {empleadoAnterior.nombreDistrito} → {model.nombreDistrito}");

                            if (empleadoAnterior.numeroTelefonico != model.numeroTelefonico)
                                cambios.Add($"Teléfono: {empleadoAnterior.numeroTelefonico} → {model.numeroTelefonico}");

                            if (empleadoAnterior.correoInstitucional != model.correoInstitucional)
                                cambios.Add($"Correo: {empleadoAnterior.correoInstitucional} → {model.correoInstitucional}");

                            if (cambios.Any())
                            {
                                var descripcionCambios = string.Join("; ", cambios);

                                _registrarEventoHistorialLN.RegistrarEvento(
                                    model.idEmpleado,
                                    "Modificación de Datos Personales",
                                    "Se actualizaron los datos personales del empleado",
                                    descripcionCambios,
                                    null,
                                    null,
                                    User.Identity.GetUserId(),
                                    Request.UserHostAddress
                                );

                                System.Diagnostics.Debug.WriteLine($"✅ Evento registrado en historial: {descripcionCambios}");
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"❌ Error al registrar evento en historial: {ex.Message}");
                        }

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
                    // Registrar evento en el historial
                    try
                    {
                        var empleadoAnterior = _datosPersonalesLN.ObtenerEmpleadoPorId(model.IdEmpleado);
                        var cambios = new List<string>();

                        if (empleadoAnterior.idCargo != model.IdCargo)
                        {
                            var cargoAnterior = _datosPersonalesLN.ObtenerCargos().FirstOrDefault(c => c.idCargo == empleadoAnterior.idCargo);
                            var cargoNuevo = _datosPersonalesLN.ObtenerCargos().FirstOrDefault(c => c.idCargo == model.IdCargo);
                            cambios.Add($"Cargo: {cargoAnterior?.nombreCargo ?? "N/A"} → {cargoNuevo?.nombreCargo ?? "N/A"}");
                        }
                        if (empleadoAnterior.fechaContratacion != model.FechaIngreso)
                            cambios.Add($"Fecha Ingreso: {empleadoAnterior.fechaContratacion:dd/MM/yyyy} → {model.FechaIngreso:dd/MM/yyyy}");

                        if (empleadoAnterior.fechaSalida != model.FechaSalida)
                            cambios.Add($"Fecha Salida: {empleadoAnterior.fechaSalida?.ToString("dd/MM/yyyy") ?? "N/A"} → {model.FechaSalida?.ToString("dd/MM/yyyy") ?? "N/A"}");

                        if (cambios.Any())
                        {
                            var descripcionCambios = string.Join("; ", cambios);

                            _registrarEventoHistorialLN.RegistrarEvento(
                                model.IdEmpleado,
                                "Cambio de Datos Laborales",
                                "Se modificaron los datos laborales del empleado",
                                descripcionCambios,
                                null,
                                null,
                                User.Identity.GetUserId(),
                                Request.UserHostAddress
                            );

                            System.Diagnostics.Debug.WriteLine($"✅ Evento laboral registrado en historial: {descripcionCambios}");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Error al registrar evento laboral en historial: {ex.Message}");
                    }

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

                    bool resultado = _datosPersonalesLN.ActualizarDatosFinancieros(
                        model.IdEmpleado,
                        model.SalarioAprobado,
                        empleadoOriginal.salarioDiario,
                        model.PeriocidadPago,
                        (int)model.IdTipoMoneda,
                        model.CuentaIBAN,
                        (int)model.IdBanco);

                    System.Diagnostics.Debug.WriteLine($"Resultado de ActualizarDatosFinancieros: {resultado}");

                    if (resultado)
                    {
                        try
                        {
                            var cambios = new List<string>();
                            if (empleadoOriginal.salarioAprobado != model.SalarioAprobado)
                                cambios.Add($"Salario: {empleadoOriginal.salarioAprobado:C} → {model.SalarioAprobado:C}");
                            if (empleadoOriginal.periocidadPago != model.PeriocidadPago)
                                cambios.Add($"Periodicidad: {empleadoOriginal.periocidadPago} → {model.PeriocidadPago}");
                            if (empleadoOriginal.idMoneda != model.IdTipoMoneda)
                                cambios.Add($"Moneda: {empleadoOriginal.nombreMoneda} → {model.TipoMoneda}");
                            if (empleadoOriginal.cuentaIBAN != model.CuentaIBAN)
                                cambios.Add($"IBAN: {empleadoOriginal.cuentaIBAN} → {model.CuentaIBAN}");
                            if (empleadoOriginal.idBanco != model.IdBanco)
                                cambios.Add($"Banco: {empleadoOriginal.nombreBanco} → {model.Banco}");

                            if (cambios.Any())
                            {
                                var descripcionCambios = string.Join("; ", cambios);
                                _registrarEventoHistorialLN.RegistrarEvento(
                                    model.IdEmpleado,
                                    "Cambio de Datos Financieros",
                                    "Se modificaron los datos financieros del empleado",
                                    descripcionCambios,
                                    null,
                                    null,
                                    User.Identity.GetUserId(),
                                    Request.UserHostAddress
                                );

                                System.Diagnostics.Debug.WriteLine($"✅ Evento financiero registrado en historial: {descripcionCambios}");
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"❌ Error al registrar evento financiero en historial: {ex.Message}");
                        }

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

        #endregion

        #region Gestión de Roles y Permisos

        [HttpGet]
        [ActiveRoleAuthorize("Administrador")]
        public async Task<JsonResult> ObtenerRolesEmpleado(int idEmpleado)
        {
            try
            {
                var empleado = _datosPersonalesLN.ObtenerEmpleadoPorId(idEmpleado);
                if (empleado == null)
                {
                    return Json(new { success = false, message = "Empleado no encontrado." }, JsonRequestBehavior.AllowGet);
                }

                var todosLosRoles = new[] { "Administrador", "Contador", "Empleado" };
                var rolesAsignados = new List<string>();

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

                var rolesData = todosLosRoles.Select(rol => new
                {
                    nombre = rol,
                    asignado = rolesAsignados.Contains(rol)
                }).ToList();

                return Json(new { success = true, roles = rolesData }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al obtener roles: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ActiveRoleAuthorize("Administrador")]
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

                if (!await RoleManager.RoleExistsAsync(nombreRol))
                {
                    return Json(new { success = false, message = "El rol especificado no existe." });
                }

                if (await UserManager.IsInRoleAsync(user.Id, nombreRol))
                {
                    return Json(new { success = false, message = "El usuario ya tiene este rol asignado." });
                }

                var result = await UserManager.AddToRoleAsync(user.Id, nombreRol);
                if (result.Succeeded)
                {
                    try
                    {
                        _registrarEventoHistorialLN.RegistrarEvento(
                            idEmpleado,
                            "Asignación de Rol",
                            $"Se asignó el rol '{nombreRol}' al empleado",
                            $"Rol asignado: {nombreRol}",
                            null,
                            nombreRol,
                            User.Identity.GetUserId(),
                            Request.UserHostAddress
                        );
                    }
                    catch { /* log */ }

                    await InvalidateUserSessions(user.Id);
                    return Json(new { success = true, message = $"Rol '{nombreRol}' asignado correctamente.", requiresRefresh = true });
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

        [HttpPost]
        [ActiveRoleAuthorize("Administrador")]
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

                if (!await UserManager.IsInRoleAsync(user.Id, nombreRol))
                {
                    return Json(new { success = false, message = "El usuario no tiene este rol asignado." });
                }

                var result = await UserManager.RemoveFromRoleAsync(user.Id, nombreRol);
                if (result.Succeeded)
                {
                    try
                    {
                        _registrarEventoHistorialLN.RegistrarEvento(
                            idEmpleado,
                            "Remoción de Rol",
                            $"Se removió el rol '{nombreRol}' del empleado",
                            $"Rol removido: {nombreRol}",
                            nombreRol,
                            null,
                            User.Identity.GetUserId(),
                            Request.UserHostAddress
                        );
                    }
                    catch { /* log */ }

                    await InvalidateUserSessions(user.Id);
                    return Json(new { success = true, message = $"Rol '{nombreRol}' removido correctamente.", requiresRefresh = true });
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

        [HttpPost]
        [ActiveRoleAuthorize("Administrador")]
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

        private string GetHighestPriorityRole(List<string> roles)
        {
            var rolesPriority = new List<string> { "Administrador", "Contador", "Empleado" };
            foreach (var priorityRole in rolesPriority)
            {
                if (roles.Contains(priorityRole))
                {
                    System.Diagnostics.Debug.WriteLine($"Rol de mayor prioridad seleccionado: {priorityRole}");
                    return priorityRole;
                }
            }
            var fallbackRole = roles.FirstOrDefault() ?? "Empleado";
            System.Diagnostics.Debug.WriteLine($"Usando rol fallback: {fallbackRole}");
            return fallbackRole;
        }

        private async Task InvalidateUserSessions(string userId)
        {
            try
            {
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

                var rolesActuales = await UserManager.GetRolesAsync(userId);
                var rolesActualesList = rolesActuales.ToList();

                var rolesEnSesion = Session["UserRoles"] as List<string> ?? new List<string>();

                bool hayDiferencias = !rolesActualesList.OrderBy(r => r).SequenceEqual(rolesEnSesion.OrderBy(r => r));

                if (hayDiferencias)
                {
                    Session["UserRoles"] = rolesActualesList;

                    var rolActivo = Session["ActiveRole"] as string;
                    if (!string.IsNullOrEmpty(rolActivo) && !rolesActualesList.Contains(rolActivo))
                    {
                        Session["ActiveRole"] = GetHighestPriorityRole(rolesActualesList);
                    }

                    return Json(new
                    {
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

        #region Role Switcher Methods

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

                Session["UserRoles"] = rolesList;

                System.Diagnostics.Debug.WriteLine($"User roles: {string.Join(", ", rolesList)}");

                var activeRole = Session["ActiveRole"] as string;
                if (string.IsNullOrEmpty(activeRole) || !rolesList.Contains(activeRole))
                {
                    activeRole = GetHighestPriorityRole(rolesList);
                    Session["ActiveRole"] = activeRole;
                }

                System.Diagnostics.Debug.WriteLine($"Active role: {activeRole}");

                var hasMultipleRoles = rolesList.Count > 1;

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

                    return Json(new
                    {
                        success = true,
                        message = $"Rol cambiado a '{role}' exitosamente.",
                        newRole = role
                    });
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"User does not have role: {role}");
                    return Json(new
                    {
                        success = false,
                        message = "No tienes permisos para acceder a este rol."
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in SwitchRole: {ex.Message}");
                return Json(new
                {
                    success = false,
                    message = "Error al cambiar el rol: " + ex.Message
                });
            }
        }

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

                var newActiveRole = GetHighestPriorityRole(rolesList);
                Session["ActiveRole"] = newActiveRole;
                Session["UserRoles"] = rolesList;

                System.Diagnostics.Debug.WriteLine($"🔄 Rol activo actualizado manualmente a: {newActiveRole}");

                return Json(new
                {
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
        // MÉTODOS AUXILIARES PARA DATOS GEOGRÁFICOS
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
    }
    #endregion
}
