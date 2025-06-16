using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Emplaniapp.Abstracciones.InterfacesParaUI.Hoja_Resumen.ListarHojaResumen;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.LogicaDeNegocio.Hoja_Resumen.ListarHojaResumen;
using Emplaniapp.Abstracciones.InterfacesParaUI;
using Emplaniapp.LogicaDeNegocio;
using Emplaniapp.UI.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Emplaniapp.Abstracciones.Entidades;

namespace Emplaniapp.UI.Controllers
{
    [Authorize(Roles = "Administrador, Contador")]
    public class HojaResumenController : Controller
    {
        private IlistarHojaResumenLN _listarHojaResumenLN;
        private IDatosPersonalesLN _datosPersonalesLN;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public HojaResumenController()
        {
            _listarHojaResumenLN = new listarHojaResumenLN();
            _datosPersonalesLN = new DatosPersonalesLN();
        }

        public HojaResumenController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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

        private List<SelectListItem> ObtenerCargos()
        {
            return _listarHojaResumenLN.ObtenerCargos()
                .Select(p => new SelectListItem
                {
                    Value = p.idCargo.ToString(),
                    Text = p.nombreCargo
                }).ToList();
        }

        private List<SelectListItem> ObtenerEstados()
        {
            return _listarHojaResumenLN.ObtenerEstados()
                .Select(e => new SelectListItem
                {
                    Value = e.idEstado.ToString(),
                    Text = e.nombreEstado
                }).ToList();
        }

        private SelectList ObtenerCargosSelectList(object selectedValue = null)
        {
            var cargos = _datosPersonalesLN.ObtenerCargos();
            return new SelectList(cargos, "idCargo", "nombreCargo", selectedValue);
        }

        private SelectList ObtenerTiposMonedasSelectList(object selectedValue = null)
        {
            var monedas = _datosPersonalesLN.ObtenerTiposMoneda();
            return new SelectList(monedas, "idMoneda", "nombreMoneda", selectedValue);
        }

        private SelectList ObtenerBancosSelectList(object selectedValue = null)
        {
            var bancos = _datosPersonalesLN.ObtenerBancos();
            return new SelectList(bancos, "idBanco", "nombreBanco", selectedValue);
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

        // GET: HojaResumen
        public ActionResult listarHojaResumen()
        {
            List<HojaResumenDto> laListaDeHojaDeResumen = _listarHojaResumenLN.ObtenerHojasResumen();
            ViewBag.Cargos = ObtenerCargos();
            ViewBag.Estados = ObtenerEstados();
            ViewBag.TotalEmpleados = _listarHojaResumenLN.ObtenerTotalEmpleados(null, null, null);
            return View(laListaDeHojaDeResumen);
        }

        // GET: HojaResumen/AgregarEmpleado
        public ActionResult AgregarEmpleado()
        {
            var model = new AgregarEmpleadoViewModel
            {
                FechaNacimiento = DateTime.Now.AddYears(-25), // Valor por defecto
                FechaContratacion = DateTime.Now,
                IdDireccion = 1, // Dirección por defecto
                IdEstado = 1     // Estado Activo por defecto
            };

            ViewBag.Cargos = ObtenerCargosSelectList();
            ViewBag.TiposMoneda = ObtenerTiposMonedasSelectList();
            ViewBag.Bancos = ObtenerBancosSelectList();
            ViewBag.PeriocidadesPago = ObtenerPeriocidadesPagoSelectList();
            ViewBag.RolesList = RoleManager.Roles.ToList().Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();

            return View(model);
        }

        // POST: HojaResumen/AgregarEmpleado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AgregarEmpleado(AgregarEmpleadoViewModel model)
        {
            // Añadimos un log para ver qué datos llegan al controlador
            System.Diagnostics.Debug.WriteLine($"Intento de crear empleado. UserName: {model.UserName}, Rol: {model.Role}");

            if (ModelState.IsValid)
            {
                // 1. Crear el usuario de ASP.NET Identity usando el UserName del modelo
                var user = new ApplicationUser { UserName = model.UserName, Email = model.CorreoInstitucional };
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
                        nombre = model.Nombre,
                        segundoNombre = model.SegundoNombre,
                        primerApellido = model.PrimerApellido,
                        segundoApellido = model.SegundoApellido,
                        fechaNacimiento = model.FechaNacimiento,
                        cedula = model.Cedula,
                        numeroTelefonico = model.NumeroTelefonico,
                        correoInstitucional = model.CorreoInstitucional,
                        idDireccion = model.IdDireccion,
                        idCargo = model.IdCargo,
                        fechaContratacion = model.FechaContratacion,
                        fechaSalida = model.FechaSalida,
                        periocidadPago = model.PeriocidadPago,
                        salarioAprobado = model.SalarioAprobado,
                        idMoneda = model.IdTipoMoneda,
                        cuentaIBAN = model.CuentaIBAN,
                        idBanco = model.IdBanco,
                        idEstado = model.IdEstado
                    };

                    // 4. Guardar el empleado en la base de datos
                    bool creacionEmpleadoExitosa = _datosPersonalesLN.CrearEmpleado(empleadoDto);

                    if (creacionEmpleadoExitosa)
                    {
                        TempData["Mensaje"] = "Empleado y usuario creados exitosamente.";
                        TempData["TipoMensaje"] = "success";
                        return RedirectToAction("listarHojaResumen");
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
            ViewBag.Cargos = ObtenerCargosSelectList(model.IdCargo);
            ViewBag.TiposMoneda = ObtenerTiposMonedasSelectList(model.IdTipoMoneda);
            ViewBag.Bancos = ObtenerBancosSelectList(model.IdBanco);
            ViewBag.PeriocidadesPago = ObtenerPeriocidadesPagoSelectList(model.PeriocidadPago);
            ViewBag.RolesList = RoleManager.Roles.ToList().Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Filtrar(string filtro, int? idCargo, int? idEstado)
        {
            var listaFiltrada = _listarHojaResumenLN.ObtenerFiltrado(filtro, idCargo, idEstado);
            ViewBag.Filtro = filtro;
            ViewBag.idCargo = idCargo;
            ViewBag.idEstado = idEstado;
            ViewBag.Cargos = ObtenerCargos();
            ViewBag.Estados = ObtenerEstados();
            ViewBag.TotalEmpleados = _listarHojaResumenLN.ObtenerTotalEmpleados(filtro, idCargo, idEstado);
            return View("listarHojaResumen", listaFiltrada);
        }

        // GET: HojaResumen/CambiarEstado/5
        public ActionResult CambiarEstado(int id)
        {
            var empleado = _listarHojaResumenLN.ObtenerEmpleadoPorId(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }

            ViewBag.Estados = _listarHojaResumenLN.ObtenerEstados()
                .Select(e => new SelectListItem
                {
                    Value = e.idEstado.ToString(),
                    Text = e.nombreEstado,
                    Selected = e.idEstado == empleado.idEstado
                }).ToList();

            return View(empleado);
        }

        // POST: HojaResumen/CambiarEstado/5
        [HttpPost]
        public ActionResult CambiarEstado(int id, int idEstado)
        {
            try
            {
                bool resultado = _listarHojaResumenLN.CambiarEstadoEmpleado(id, idEstado);
                if (resultado)
                {
                    TempData["Mensaje"] = "Estado del empleado cambiado exitosamente.";
                    TempData["TipoMensaje"] = "success";
                }
                else
                {
                    TempData["Mensaje"] = "Error al cambiar el estado del empleado.";
                    TempData["TipoMensaje"] = "error";
                }
                return RedirectToAction("listarHojaResumen");
            }
            catch
            {
                TempData["Mensaje"] = "Error al cambiar el estado del empleado.";
                TempData["TipoMensaje"] = "error";
                return RedirectToAction("listarHojaResumen");
            }
        }

        // GET: HojaResumen/VerDetalles/5
        public ActionResult VerDetalles(int id)
        {
            // Redirige al controlador de DatosPersonales para ver los detalles del empleado
            return RedirectToAction("Detalles", "DatosPersonales", new { id = id });
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

        // GET: HojaResumen/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HojaResumen/Create
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

        // GET: HojaResumen/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HojaResumen/Edit/5
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

        // GET: HojaResumen/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HojaResumen/Delete/5
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

        /** #region Métodos Auxiliares

        // Método para obtener la lista de empleados (simulado)
        private List<EmpleadoDto> ObtenerEmpleados()
        {
            // Crear una lista de empleados (datos de ejemplo)
            return new List<EmpleadoDto>
            {
                new EmpleadoDto
                {
                    idEmpleado = 1,
                    nombre = "Yazmin",
                    segundoNombre = "",
                    primerApellido = "Rivera",
                    segundoApellido = "Rodríguez",
                    fechaNacimiento = new DateTime(1992, 11, 23),
                    cedula = 128048070,
                    numeroTelefonico = "7821-9903",
                    correoInstitucional = "yazriv@gmail.com",
                    idDireccion = 1,
                    idCargo = 2,
                    nombreCargo = "Contador",
                    fechaContratacion = new DateTime(2021, 9, 9),
                    fechaSalida = null,
                    periocidadPago = "Quincenal",
                    salarioDiario = 13400,
                    salarioAprobado = 1545000,
                    salarioPorMinuto = 27.91,
                    salarioPoHora = 1675.00,
                    salarioPorHoraExtra = 3350.00,
                    idMoneda = 1,
                    nombreMoneda = "Colones",
                    cuentaIBAN = "3456789087456789098890987",
                    idBanco = 1,
                    nombreBanco = "BN",
                    idEstado = 1,
                    nombreEstado = "Activo"
                },
                new EmpleadoDto
                {
                    idEmpleado = 2,
                    nombre = "Juan",
                    segundoNombre = "Carlos",
                    primerApellido = "Pérez",
                    segundoApellido = "Gómez",
                    fechaNacimiento = new DateTime(1990, 5, 15),
                    cedula = 123456789,
                    numeroTelefonico = "8888-8888",
                    correoInstitucional = "juan.perez@example.com",
                    idDireccion = 2,
                    idCargo = 1,
                    nombreCargo = "Administrador",
                    fechaContratacion = new DateTime(2020, 1, 10),
                    fechaSalida = null,
                    periocidadPago = "Mensual",
                    salarioDiario = 100000,
                    salarioAprobado = 3000000,
                    salarioPorMinuto = 0.1,
                    salarioPoHora = 10000,
                    salarioPorHoraExtra = 15000,
                    idMoneda = 1,
                    nombreMoneda = "Colones",
                    cuentaIBAN = "CR123456789012345678",
                    idBanco = 2,
                    nombreBanco = "BCR",
                    idEstado = 1,
                    nombreEstado = "Activo"
                },
                new EmpleadoDto
                {
                    idEmpleado = 3,
                    nombre = "María",
                    segundoNombre = "José",
                    primerApellido = "Rodríguez",
                    segundoApellido = "Castro",
                    fechaNacimiento = new DateTime(1985, 10, 20),
                    cedula = 987654321,
                    numeroTelefonico = "7777-7777",
                    correoInstitucional = "maria.rodriguez@example.com",
                    idDireccion = 3,
                    idCargo = 3,
                    nombreCargo = "Gerente",
                    fechaContratacion = new DateTime(2018, 3, 1),
                    fechaSalida = null,
                    periocidadPago = "Quincenal",
                    salarioDiario = 80000,
                    salarioAprobado = 2400000,
                    salarioPorMinuto = 0.08,
                    salarioPoHora = 8000,
                    salarioPorHoraExtra = 12000,
                    idMoneda = 1,
                    nombreMoneda = "Colones",
                    cuentaIBAN = "CR987654321098765432",
                    idBanco = 3,
                    nombreBanco = "BAC",
                    idEstado = 1,
                    nombreEstado = "Activo"
                }
            };
        }

        #endregion */
    }
}

