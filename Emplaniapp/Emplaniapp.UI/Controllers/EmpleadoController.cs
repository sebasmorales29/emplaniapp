using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Emplaniapp.Abstracciones.Entidades;
using Emplaniapp.Abstracciones.InterfacesParaUI.Bancos.ListarBancos;
using Emplaniapp.Abstracciones.InterfacesParaUI.Cargos.ListarCargos;
using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.AgregarEmpleado;
using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.ListarEmpleado;
using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.ModificarEstado;
using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.Abstracciones.InterfacesParaUI.Estados.ListarEstados;
using Emplaniapp.Abstracciones.InterfacesParaUI.General.FiltrarEmpleados;
using Emplaniapp.Abstracciones.InterfacesParaUI.General.ObtenerTotalEmpleados;
using Emplaniapp.Abstracciones.InterfacesParaUI.Monedas.ListarMonedas;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.LogicaDeNegocio.Bancos.ListarBancos;
using Emplaniapp.LogicaDeNegocio.Cargos.ListarCargos;
using Emplaniapp.LogicaDeNegocio.Empleado.AgregarEmpleado;
using Emplaniapp.LogicaDeNegocio.Empleado.ListarEmpleado;
using Emplaniapp.LogicaDeNegocio.Empleado.ModificarEstado;
using Emplaniapp.LogicaDeNegocio.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.LogicaDeNegocio.Estados.ListarEstados;
using Emplaniapp.LogicaDeNegocio.General.FiltrarEmpleados;
using Emplaniapp.LogicaDeNegocio.General.ObtenerTotalEmpleados;
using Emplaniapp.LogicaDeNegocio.Monedas.ListarMonedas;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Emplaniapp.AccesoADatos;

namespace Emplaniapp.UI.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class EmpleadoController : Controller
    {
        private IListarEmpleadoLN _listarEmpleadoLN;
        private IAgregarEmpleadoLN _agregarEmpleadoLN;
        private IListarCargosLN _listarCargosLN;
        private IListarBancosLN _listarBancosLN;
        private IlistarMonedasLN _listarMonedasLN;
        private IModificarEstadoLN _modificarEstadoLN;
        private IListarEstadosLN _listarEstadosLN;
        private IObtenerEmpleadoPorIdLN _obtenerEmpleadoLN;
        private IFiltrarEmpleadosLN _filtrarEmpleadosLN;
        private IObtenerTotalEmpleadosLN _obtenerTotalEmpleadosLN;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public EmpleadoController()
        {
            _listarEmpleadoLN = new listarEmpleadoLN();
            _agregarEmpleadoLN = new agregarEmpleadoLN();
            _listarCargosLN = new listarCargosLN();
            _listarBancosLN = new listarBancosLN();
            _listarMonedasLN = new listarMonedasLN();
            _modificarEstadoLN = new modificarEstadoLN();
            _listarEstadosLN = new listarEstadosLN();
            _obtenerEmpleadoLN = new ObtenerEmpleadoPorIdLN();
            _filtrarEmpleadosLN = new filtrarEmpleadosLN();
            _obtenerTotalEmpleadosLN = new obtenerTotalEmpleadosLN();

        }

        public EmpleadoController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
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

        private SelectList ObtenerCargosSelectList(int? selectedValue = null)
        {
            var cargos = _listarCargosLN.ObtenerCargos();
            return new SelectList(cargos, "idCargo", "nombreCargo", selectedValue);
        }

        private SelectList ObtenerTiposMonedasSelectList(int? selectedValue = null)
        {
            var monedas = _listarMonedasLN.ObtenerMonedas();
            return new SelectList(monedas, "idMoneda", "nombreMoneda", selectedValue);
        }

        private SelectList ObtenerBancosSelectList(int? selectedValue = null)
        {
            var bancos = _listarBancosLN.ObtenerBancos();
            return new SelectList(bancos, "idBanco", "nombreBanco", selectedValue);
        }

        private SelectList ObtenerPeriocidadesPagoSelectList(string selectedValue = null)
        {
            var periodicidades = new[]
            {
                new { Value = "Quincenal", Text = "Quincenal" },
                new { Value = "Mensual", Text = "Mensual" }
            };

            return new SelectList(periodicidades, "Value", "Text", selectedValue);
        }

        // ===============================================
        // MÉTODOS PARA DATOS GEOGRÁFICOS
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
        // GET: Empleado
        public ActionResult ListarEmpleados()
        {
            List<EmpleadoDto> laListaDeEmpleados = _listarEmpleadoLN.ObtenerEmpleados();
            ViewBag.Cargos = ObtenerCargos();
            ViewBag.Estados = ObtenerEstados();
            ViewBag.TotalEmpleados = _obtenerTotalEmpleadosLN.ObtenerTotalEmpleados(null, null, null, true);
            return View(laListaDeEmpleados);
        }

        private List<SelectListItem> ObtenerCargos()
        {
            return _listarCargosLN.ObtenerCargos()
                .Select(p => new SelectListItem
                {
                    Value = p.idCargo.ToString(),
                    Text = p.nombreCargo
                }).ToList();
        }
        private List<SelectListItem> ObtenerEstados()
        {
            return _listarEstadosLN.ObtenerEstados()
                .Select(p => new SelectListItem
                {
                    Value = p.idEstado.ToString(),
                    Text = p.nombreEstado
                }).ToList();
        }

        [HttpPost]
        public ActionResult Filtrar(string filtro, int? idCargo, int? idEstado)
        {
            var listaFiltrada = _filtrarEmpleadosLN.ObtenerFiltrado<EmpleadoDto>(filtro, idCargo, idEstado);
            ViewBag.Filtro = filtro;
            ViewBag.idCargo = idCargo;
            ViewBag.idEstado = idEstado;
            ViewBag.Cargos = ObtenerCargos();
            ViewBag.Estados = ObtenerEstados();
            ViewBag.TotalEmpleados = _obtenerTotalEmpleadosLN.ObtenerTotalEmpleados(filtro, idCargo, idEstado, false);
            return View("ListarEmpleados", listaFiltrada);
        }

        // GET: Empleado/Details/5
        public ActionResult DetalleEmpleado(int id)
        {
            return View();
        }

        // GET: Empleado/Create
        public ActionResult CrearEmpleado()
        {
            var model = new EmpleadoDto
            {
                fechaNacimiento = DateTime.Now.AddYears(-25), // Valor por defecto
                fechaContratacion = DateTime.Now,
                idProvincia = 1, 
                idDistrito = 1, // Valores por defecto
                idCanton = 1, // Valores por defecto
                idCalle = 1, // Valores por defecto
                direccionDetallada = "Dirección por defecto", // Valor por defecto
                idEstado = 1     // Estado Activo por defecto
            };

            ViewBag.Cargos = ObtenerCargosSelectList();
            ViewBag.TiposMoneda = ObtenerTiposMonedasSelectList();
            ViewBag.Bancos = ObtenerBancosSelectList();
            ViewBag.PeriocidadesPago = ObtenerPeriocidadesPagoSelectList();
            
            // Cargar datos geográficos para dropdowns en cascada
            ViewBag.Provincias = ObtenerProvinciasSelectList(model.idProvincia);
            ViewBag.Cantones = ObtenerCantonesSelectList(model.idProvincia, model.idCanton);
            ViewBag.Distritos = ObtenerDistritosSelectList(model.idCanton, model.idDistrito);
            ViewBag.Calles = ObtenerCallesSelectList(model.idDistrito, model.idCalle);
            
            ViewBag.RolesList = RoleManager.Roles.ToList().Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();
            return View(model);
        }

        // POST: Empleado/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CrearEmpleado(EmpleadoDto model)
        {
            try
            {
                // Añadimos un log para ver qué datos llegan al controlador
                System.Diagnostics.Debug.WriteLine($"Intento de crear empleado. UserName: {model.UserName}, Rol: {model.Role}");

                if (ModelState.IsValid)
                {
                    // 1. Crear el usuario de ASP.NET Identity usando el UserName del modelo
                    var user = new ApplicationUser { UserName = model.UserName, Email = model.correoInstitucional };
                    var result = await UserManager.CreateAsync(user, model.Password);

                    // --- INICIO DE CÓDIGO DE DEPURACIÓN ---
                    if (result.Succeeded)
                    {
                        System.Diagnostics.Debug.WriteLine($"ÉXITO: Usuario de Identity '{user.UserName}' (ID: {user.Id}) creado correctamente.");
                        // --- FIN DE CÓDIGO DE DEPURACIÓN ---

                        // 2. Asignar rol al usuario
                        await UserManager.AddToRoleAsync(user.Id, model.Role);
                        System.Diagnostics.Debug.WriteLine($"ÉXITO: Rol '{model.Role}' asignado al usuario '{user.UserName}'.");

                        // 3. Crear el DTO del empleado para la lógica de negocio
                        var empleadoDto = new EmpleadoDto
                        {
                            // Enlazar el usuario de Identity con el empleado
                            IdNetUser = user.Id,

                            // Datos personales del formulario
                            nombre = model.nombre?.Trim(),
                            segundoNombre = string.IsNullOrWhiteSpace(model.segundoNombre) ? null : model.segundoNombre.Trim(),
                            primerApellido = model.primerApellido?.Trim(),
                            segundoApellido = model.segundoApellido?.Trim(),
                            fechaNacimiento = model.fechaNacimiento,
                            cedula = model.cedula,
                            numeroTelefonico = model.numeroTelefonico?.Trim(),
                            correoInstitucional = model.correoInstitucional?.Trim(),
                            
                            // Datos de ubicación con valores por defecto seguros
                            idProvincia = 1, // San José por defecto
                            idCanton = 1,    // San José por defecto  
                            idDistrito = 1,  // Carmen por defecto
                            idCalle = 1,     // Calle por defecto
                            direccionDetallada = string.IsNullOrWhiteSpace(model.direccionDetallada) ? "Dirección por definir" : model.direccionDetallada.Trim(),
                            
                            // Datos laborales
                            idCargo = model.idCargo.HasValue ? model.idCargo.Value : 1, // Validar que idCargo no sea null
                            fechaContratacion = model.fechaContratacion,
                            periocidadPago = model.periocidadPago,
                            salarioAprobado = model.salarioAprobado,
                            
                            // Datos bancarios
                            idMoneda = model.idMoneda.HasValue ? model.idMoneda.Value : 1, // Colón por defecto
                            cuentaIBAN = model.cuentaIBAN?.Trim(),
                            idBanco = model.idBanco.HasValue ? model.idBanco.Value : 1, // Banco por defecto
                            
                            // Estado
                            idEstado = 1 // Activo por defecto
                        };

                        System.Diagnostics.Debug.WriteLine("📋 DTO del empleado creado:");
                        System.Diagnostics.Debug.WriteLine($"IdNetUser: {empleadoDto.IdNetUser}");
                        System.Diagnostics.Debug.WriteLine($"nombre: {empleadoDto.nombre}");
                        System.Diagnostics.Debug.WriteLine($"cedula: {empleadoDto.cedula}");
                        System.Diagnostics.Debug.WriteLine($"correoInstitucional: {empleadoDto.correoInstitucional}");
                        System.Diagnostics.Debug.WriteLine($"idProvincia: {empleadoDto.idProvincia}");
                        System.Diagnostics.Debug.WriteLine($"idCanton: {empleadoDto.idCanton}");
                        System.Diagnostics.Debug.WriteLine($"idDistrito: {empleadoDto.idDistrito}");
                        System.Diagnostics.Debug.WriteLine($"idCalle: {empleadoDto.idCalle}");
                        System.Diagnostics.Debug.WriteLine($"direccionDetallada: {empleadoDto.direccionDetallada}");
                        System.Diagnostics.Debug.WriteLine($"idCargo: {empleadoDto.idCargo}");
                        System.Diagnostics.Debug.WriteLine($"periocidadPago: {empleadoDto.periocidadPago}");
                        System.Diagnostics.Debug.WriteLine($"salarioAprobado: {empleadoDto.salarioAprobado}");
                        System.Diagnostics.Debug.WriteLine($"idMoneda: {empleadoDto.idMoneda}");
                        System.Diagnostics.Debug.WriteLine($"idBanco: {empleadoDto.idBanco}");

                        // 4. Verificar que existan los datos básicos necesarios
                        string validationError = ValidarDatosBasicos(empleadoDto);
                        if (!string.IsNullOrEmpty(validationError))
                        {
                            await UserManager.DeleteAsync(user);
                            ModelState.AddModelError("", $"Error de datos básicos: {validationError}");
                            System.Diagnostics.Debug.WriteLine($"❌ ERROR DE VALIDACIÓN: {validationError}");
                        }
                        else
                        {
                            // 5. Guardar el empleado en la base de datos
                            System.Diagnostics.Debug.WriteLine("🚀 Llamando a CrearEmpleado...");
                            bool creacionEmpleadoExitosa = _agregarEmpleadoLN.CrearEmpleado(empleadoDto);
                            System.Diagnostics.Debug.WriteLine($"🎯 Resultado CrearEmpleado: {creacionEmpleadoExitosa}");

                            if (creacionEmpleadoExitosa)
                            {
                                TempData["Mensaje"] = "Empleado y usuario creados exitosamente.";
                                TempData["TipoMensaje"] = "success";
                                return RedirectToAction("listarEmpleados");
                            }
                            else
                            {
                                // Si falla la creación del empleado, hay que borrar el usuario que ya creamos para no dejar datos huérfanos.
                                await UserManager.DeleteAsync(user);
                                ModelState.AddModelError("", "Hubo un error al guardar los datos del empleado.");
                                System.Diagnostics.Debug.WriteLine("❌ ERROR: Falló la creación del EMPLEADO en la BD, se ha borrado el usuario de Identity para evitar datos huérfanos.");
                            }
                        }
                    }
                    else
                    {
                        // --- INICIO DE CÓDIGO DE DEPURACIÓN ---
                        System.Diagnostics.Debug.WriteLine($"ERROR: Falló la creación del usuario de Identity '{model.UserName}'. Razones:");
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error);
                            System.Diagnostics.Debug.WriteLine($"- {error}");
                        }
                        // --- FIN DE CÓDIGO DE DEPURACIÓN ---
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: ModelState no es válido.");
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var error in errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"- {error.ErrorMessage}");
                    }
                }

                // Si llegamos aquí, algo falló. Recargamos los dropdowns y devolvemos la vista.
                ViewBag.Cargos = ObtenerCargosSelectList(model.idCargo);
                ViewBag.TiposMoneda = ObtenerTiposMonedasSelectList(model.idMoneda);
                ViewBag.Bancos = ObtenerBancosSelectList(model.idBanco);
                ViewBag.PeriocidadesPago = ObtenerPeriocidadesPagoSelectList(model.periocidadPago);
                
                // Recargar datos geográficos
                ViewBag.Provincias = ObtenerProvinciasSelectList(model.idProvincia);
                ViewBag.Cantones = ObtenerCantonesSelectList(model.idProvincia, model.idCanton);
                ViewBag.Distritos = ObtenerDistritosSelectList(model.idCanton, model.idDistrito);
                ViewBag.Calles = ObtenerCallesSelectList(model.idDistrito, model.idCalle);
                
                ViewBag.RolesList = RoleManager.Roles.ToList().Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();

                return View(model);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
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

        [HttpGet]
        public ActionResult _CambiarEstado(int id)
        {
            var empleado = _obtenerEmpleadoLN.ObtenerEmpleadoPorId(id);

            if (empleado == null)
            {
                return HttpNotFound();
            }

            ViewBag.Estados = new SelectList(_listarEstadosLN.ObtenerEstados(), "idEstado", "nombreEstado");

            return PartialView("_CambiarEstado", empleado);
        }

        [HttpPost]
        public ActionResult _CambiarEstado(int id, int idEstado)
        {
            try
            {
                bool resultado = _modificarEstadoLN.CambiarEstadoEmpleado(id, idEstado);
                TempData["Mensaje"] = resultado ? "Estado actualizado correctamente." : "Error al actualizar el estado.";
                TempData["TipoMensaje"] = resultado ? "success" : "error";
            }
            catch
            {
                TempData["Mensaje"] = "Error inesperado.";
                TempData["TipoMensaje"] = "error";
            }

            return RedirectToAction("ListarEmpleados");
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

                    return Json(calles, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error obteniendo calles: {ex.Message}");
                return Json(new List<object>(), JsonRequestBehavior.AllowGet);
            }
        }

        // ===============================================
        // MÉTODOS DE VALIDACIÓN
        // ===============================================

        private string ValidarDatosBasicos(EmpleadoDto empleado)
        {
            try
            {
                using (var contexto = new Contexto())
                {
                    var errores = new List<string>();

                    // Verificar Provincia
                    if (empleado.idProvincia.HasValue)
                    {
                        var provinciaExiste = contexto.Provincia.Any(p => p.idProvincia == empleado.idProvincia.Value);
                        if (!provinciaExiste)
                            errores.Add($"Provincia con ID {empleado.idProvincia} no existe");
                    }

                    // Verificar Cantón
                    if (empleado.idCanton.HasValue)
                    {
                        var cantonExiste = contexto.Canton.Any(c => c.idCanton == empleado.idCanton.Value);
                        if (!cantonExiste)
                            errores.Add($"Cantón con ID {empleado.idCanton} no existe");
                    }

                    // Verificar Distrito
                    if (empleado.idDistrito.HasValue)
                    {
                        var distritoExiste = contexto.Distrito.Any(d => d.idDistrito == empleado.idDistrito.Value);
                        if (!distritoExiste)
                            errores.Add($"Distrito con ID {empleado.idDistrito} no existe");
                    }

                    // Verificar Calle
                    if (empleado.idCalle.HasValue)
                    {
                        var calleExiste = contexto.Calle.Any(c => c.idCalle == empleado.idCalle.Value);
                        if (!calleExiste)
                            errores.Add($"Calle con ID {empleado.idCalle} no existe");
                    }

                    // Verificar Cargo
                    if (empleado.idCargo.HasValue)
                    {
                        var cargoExiste = contexto.Cargos.Any(c => c.idCargo == empleado.idCargo.Value);
                        if (!cargoExiste)
                            errores.Add($"Cargo con ID {empleado.idCargo} no existe");
                    }

                    // Verificar Moneda
                    if (empleado.idMoneda.HasValue)
                    {
                        var monedaExiste = contexto.TipoMoneda.Any(m => m.idTipoMoneda == empleado.idMoneda.Value);
                        if (!monedaExiste)
                            errores.Add($"Tipo de moneda con ID {empleado.idMoneda} no existe");
                    }

                    // Verificar Banco
                    if (empleado.idBanco.HasValue)
                    {
                        var bancoExiste = contexto.Bancos.Any(b => b.idBanco == empleado.idBanco.Value);
                        if (!bancoExiste)
                            errores.Add($"Banco con ID {empleado.idBanco} no existe");
                    }

                    // Verificar Estado
                    var estadoExiste = contexto.Estado.Any(e => e.idEstado == empleado.idEstado);
                    if (!estadoExiste)
                        errores.Add($"Estado con ID {empleado.idEstado} no existe");

                    // Verificar que existe dirección por defecto (ID = 1)
                    var direccionExiste = contexto.Direccion.Any(d => d.idDireccion == 1);
                    if (!direccionExiste)
                        errores.Add("Dirección por defecto (ID = 1) no existe en la BD");

                    if (errores.Any())
                    {
                        System.Diagnostics.Debug.WriteLine("🔍 ERRORES DE VALIDACIÓN ENCONTRADOS:");
                        foreach (var error in errores)
                        {
                            System.Diagnostics.Debug.WriteLine($"  ❌ {error}");
                        }
                        return string.Join("; ", errores);
                    }

                    System.Diagnostics.Debug.WriteLine("✅ Validación de datos básicos EXITOSA");
                    return null; // Sin errores
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ ERROR en ValidarDatosBasicos: {ex.Message}");
                return $"Error de validación: {ex.Message}";
            }
        }
    }
}