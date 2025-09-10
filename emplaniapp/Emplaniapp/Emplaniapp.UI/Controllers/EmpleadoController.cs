﻿using System;
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
using Emplaniapp.UI.Attributes;

namespace Emplaniapp.UI.Controllers
{
    [ActiveRoleAuthorize("Administrador")]
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

        // Helper para cargar SIEMPRE los combos necesarios de CrearEmpleado
        private void CargarCombosEmpleado(EmpleadoDto model)
        {
            ViewBag.Cargos = ObtenerCargosSelectList(model.idCargo);
            ViewBag.TiposMoneda = ObtenerTiposMonedasSelectList(model.idMoneda);
            ViewBag.Bancos = ObtenerBancosSelectList(model.idBanco);
            ViewBag.PeriocidadesPago = ObtenerPeriocidadesPagoSelectList(model.periocidadPago);
            ViewBag.Provincias = ObtenerProvinciasSelectList(model.idProvincia);
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
                fechaNacimiento = DateTime.Now.AddYears(-25),
                fechaContratacion = DateTime.Now,
                idProvincia = 1,
                nombreCanton = "San José",
                nombreDistrito = "Carmen",
                direccionDetallada = "Dirección por defecto",
                idEstado = 1
            };

            CargarCombosEmpleado(model);
            return View(model);
        }

        // POST: Empleado/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CrearEmpleado(EmpleadoDto model)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Intento de crear empleado. UserName: {model.UserName}, Rol por defecto: Empleado");

                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser { UserName = model.UserName, Email = model.correoInstitucional };
                    var result = await UserManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        System.Diagnostics.Debug.WriteLine($"ÉXITO: Usuario de Identity '{user.UserName}' (ID: {user.Id}) creado correctamente.");

                        const string rolPorDefecto = "Empleado";
                        await UserManager.AddToRoleAsync(user.Id, rolPorDefecto);
                        System.Diagnostics.Debug.WriteLine($"ÉXITO: Rol por defecto '{rolPorDefecto}' asignado al usuario '{user.UserName}'.");

                        var empleadoDto = new EmpleadoDto
                        {
                            IdNetUser = user.Id,
                            nombre = model.nombre?.Trim(),
                            segundoNombre = string.IsNullOrWhiteSpace(model.segundoNombre) ? null : model.segundoNombre.Trim(),
                            primerApellido = model.primerApellido?.Trim(),
                            segundoApellido = model.segundoApellido?.Trim(),
                            fechaNacimiento = model.fechaNacimiento,
                            cedula = model.cedula,
                            numeroTelefonico = model.numeroTelefonico?.Trim(),
                            correoInstitucional = model.correoInstitucional?.Trim(),
                            idProvincia = model.idProvincia ?? 1,
                            nombreCanton = string.IsNullOrWhiteSpace(model.nombreCanton) ? "San José" : model.nombreCanton.Trim(),
                            nombreDistrito = string.IsNullOrWhiteSpace(model.nombreDistrito) ? "Carmen" : model.nombreDistrito.Trim(),
                            direccionDetallada = string.IsNullOrWhiteSpace(model.direccionDetallada) ? "Dirección por definir" : model.direccionDetallada.Trim(),
                            idCargo = model.idCargo.HasValue ? model.idCargo.Value : 1,
                            fechaContratacion = model.fechaContratacion,
                            periocidadPago = model.periocidadPago,
                            salarioAprobado = model.salarioAprobado,
                            idMoneda = model.idMoneda.HasValue ? model.idMoneda.Value : 1,
                            cuentaIBAN = model.cuentaIBAN?.Trim(),
                            idBanco = model.idBanco.HasValue ? model.idBanco.Value : 1,
                            idEstado = 1
                        };

                        string validationError = ValidarDatosBasicos(empleadoDto);
                        if (!string.IsNullOrEmpty(validationError))
                        {
                            await UserManager.DeleteAsync(user);
                            ModelState.AddModelError("", $"Error de datos básicos: {validationError}");
                        }
                        else
                        {
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
                                await UserManager.DeleteAsync(user);
                                ModelState.AddModelError("", "Hubo un error al guardar los datos del empleado. Verifique que: 1) La cédula no esté duplicada, 2) El correo electrónico no esté duplicado, 3) Todos los datos sean válidos.");
                            }
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"ERROR: Falló la creación del usuario de Identity '{model.UserName}'. Razones:");
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error);
                            System.Diagnostics.Debug.WriteLine($"- {error}");
                        }
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

                // Si aquí no hubo Redirect, recarga combos y devuelve la vista
                CargarCombosEmpleado(model);
                return View(model);
            }
            catch
            {
                // Antes devolvías View() sin modelo ni combos -> provoca el error del ViewData.
                CargarCombosEmpleado(model);
                return View(model);
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
        // MÉTODOS DE VALIDACIÓN
        // ===============================================

        private string ValidarDatosBasicos(EmpleadoDto empleado)
        {
            try
            {
                using (var contexto = new Contexto())
                {
                    var errores = new List<string>();

                    if (empleado.idProvincia.HasValue)
                    {
                        var provinciaExiste = contexto.Provincia.Any(p => p.idProvincia == empleado.idProvincia.Value);
                        if (!provinciaExiste)
                            errores.Add($"Provincia con ID {empleado.idProvincia} no existe");
                    }

                    if (string.IsNullOrWhiteSpace(empleado.nombreCanton))
                    {
                        errores.Add("El nombre del cantón es obligatorio");
                    }

                    if (string.IsNullOrWhiteSpace(empleado.nombreDistrito))
                    {
                        errores.Add("El nombre del distrito es obligatorio");
                    }

                    if (empleado.idCargo.HasValue)
                    {
                        var cargoExiste = contexto.Cargos.Any(c => c.idCargo == empleado.idCargo.Value);
                        if (!cargoExiste)
                            errores.Add($"Cargo con ID {empleado.idCargo} no existe");
                    }

                    if (empleado.idBanco.HasValue)
                    {
                        var bancoExiste = contexto.Bancos.Any(b => b.idBanco == empleado.idBanco.Value);
                        if (!bancoExiste)
                            errores.Add($"Banco con ID {empleado.idBanco} no existe");
                    }

                    if (empleado.idMoneda.HasValue)
                    {
                        var monedaExiste = contexto.TipoMoneda.Any(t => t.idTipoMoneda == empleado.idMoneda.Value);
                        if (!monedaExiste)
                            errores.Add($"TipoMoneda con ID {empleado.idMoneda} no existe");
                    }

                    var estadoExiste = contexto.Estado.Any(e => e.idEstado == empleado.idEstado);
                    if (!estadoExiste)
                        errores.Add($"Estado con ID {empleado.idEstado} no existe");

                    var direccionExiste = contexto.Direccion.Any(d => d.idDireccion == 1);
                    if (!direccionExiste)
                        errores.Add("Dirección con ID 1 no existe");

                    return string.Join("; ", errores);
                }
            }
            catch (Exception ex)
            {
                return $"Error al validar datos básicos: {ex.Message}";
            }
        }

        [HttpGet]
        public JsonResult VerificarConexionBD()
        {
            try
            {
                using (var contexto = new Contexto())
                {
                    var resultado = new
                    {
                        conexionExitosa = true,
                        datos = new
                        {
                            provincias = contexto.Provincia.Count(),
                            cargos = contexto.Cargos.Count(),
                            bancos = contexto.Bancos.Count(),
                            tiposMoneda = contexto.TipoMoneda.Count(),
                            estados = contexto.Estado.Count(),
                            direcciones = contexto.Direccion.Count(),
                            empleados = contexto.Empleados.Count(),
                            usuarios = contexto.Users.Count()
                        },
                        mensaje = "Conexión exitosa"
                    };

                    return Json(resultado, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var resultado = new
                {
                    conexionExitosa = false,
                    error = ex.Message,
                    stackTrace = ex.StackTrace,
                    mensaje = "Error de conexión a la base de datos"
                };

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult VerificarDatosEmpleado(EmpleadoDto empleado)
        {
            try
            {
                var resultado = new
                {
                    exito = true,
                    datos = new
                    {
                        nombre = empleado.nombre,
                        cedula = empleado.cedula,
                        correo = empleado.correoInstitucional,
                        idNetUser = empleado.IdNetUser,
                        idProvincia = empleado.idProvincia,
                        idCargo = empleado.idCargo,
                        idBanco = empleado.idBanco,
                        idMoneda = empleado.idMoneda,
                        idEstado = empleado.idEstado,
                        nombreCanton = empleado.nombreCanton,
                        nombreDistrito = empleado.nombreDistrito,
                        salarioAprobado = empleado.salarioAprobado,
                        periocidadPago = empleado.periocidadPago,
                        fechaNacimiento = empleado.fechaNacimiento,
                        fechaContratacion = empleado.fechaContratacion
                    },
                    validaciones = new
                    {
                        nombreValido = !string.IsNullOrWhiteSpace(empleado.nombre),
                        cedulaValida = empleado.cedula >= 100000000 && empleado.cedula <= 999999999,
                        correoValido = !string.IsNullOrWhiteSpace(empleado.correoInstitucional),
                        idNetUserValido = true,
                        mayorDeEdad = empleado.fechaNacimiento <= DateTime.Now.AddYears(-18),
                        fechaContratacionValida = empleado.fechaContratacion <= DateTime.Now,
                        salarioValido = empleado.salarioAprobado > 0,
                        periodicidadValida = empleado.periocidadPago == "Quincenal" || empleado.periocidadPago == "Mensual"
                    },
                    mensaje = "Datos verificados correctamente. Nota: El IdNetUser se genera automáticamente al crear el usuario."
                };

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var resultado = new
                {
                    exito = false,
                    error = ex.Message,
                    stackTrace = ex.StackTrace,
                    mensaje = "Error al verificar datos del empleado"
                };

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
