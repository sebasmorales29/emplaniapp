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
using Emplaniapp.Abstracciones.InterfacesParaUI.Monedas.ListarMonedas;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.LogicaDeNegocio.Bancos.ListarBancos;
using Emplaniapp.LogicaDeNegocio.Cargos.ListarCargos;
using Emplaniapp.LogicaDeNegocio.Empleado.AgregarEmpleado;
using Emplaniapp.LogicaDeNegocio.Empleado.ListarEmpleado;
using Emplaniapp.LogicaDeNegocio.Empleado.ModificarEstado;
using Emplaniapp.LogicaDeNegocio.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.LogicaDeNegocio.Estados.ListarEstados;
using Emplaniapp.LogicaDeNegocio.Monedas.ListarMonedas;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

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
        // GET: Empleado
        public ActionResult ListarEmpleados()
        {
            List<EmpleadoDto> laListaDeEmpleados = _listarEmpleadoLN.ObtenerEmpleados();
            return View(laListaDeEmpleados);
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
                idDireccion = 1, // Dirección por defecto
                idEstado = 1     // Estado Activo por defecto
            };

            ViewBag.Cargos = ObtenerCargosSelectList();
            ViewBag.TiposMoneda = ObtenerTiposMonedasSelectList();
            ViewBag.Bancos = ObtenerBancosSelectList();
            ViewBag.PeriocidadesPago = ObtenerPeriocidadesPagoSelectList();
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

                            // Datos del formulario
                            nombre = model.nombre,
                            segundoNombre = model.segundoNombre,
                            primerApellido = model.primerApellido,
                            segundoApellido = model.segundoApellido,
                            fechaNacimiento = model.fechaNacimiento,
                            cedula = model.cedula,
                            numeroTelefonico = model.numeroTelefonico,
                            correoInstitucional = model.correoInstitucional,
                            idDireccion = 1,
                            idCargo = model.idCargo,
                            fechaContratacion = model.fechaContratacion,
                            periocidadPago = model.periocidadPago,
                            salarioAprobado = model.salarioAprobado,
                            idMoneda = model.idMoneda,
                            cuentaIBAN = model.cuentaIBAN,
                            idBanco = model.idBanco,
                            idEstado = 1
                        };

                        // 4. Guardar el empleado en la base de datos
                        bool creacionEmpleadoExitosa = _agregarEmpleadoLN.CrearEmpleado(empleadoDto);

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
                            System.Diagnostics.Debug.WriteLine("ERROR: Falló la creación del EMPLEADO en la BD, se ha borrado el usuario de Identity para evitar datos huérfanos.");
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
    }
}
