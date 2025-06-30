using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Emplaniapp.Abstracciones.InterfacesParaUI;
using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones;
using Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones.CrearRemuneraciones;
using Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones.EditarRemuneracion;
using Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones.EliminarRemuneracion;
using Emplaniapp.Abstracciones.InterfacesParaUI.Tipo_Remuneracion;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.LogicaDeNegocio;
using Emplaniapp.LogicaDeNegocio.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.LogicaDeNegocio.Remuneraciones;
using Emplaniapp.LogicaDeNegocio.Remuneraciones.CrearRemuneraciones;
using Emplaniapp.LogicaDeNegocio.Remuneraciones.EditarRemuneracion;
using Emplaniapp.LogicaDeNegocio.Remuneraciones.EliminarRemuneracion;
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
        private IEliminarRemuneracionLN _eliminarRemuneracionLN;
        private IEditarRemuneracionLN _editarRemuneracionLN;
        private ApplicationUserManager _userManager;

        // Constructores ------------------------------------------------------------------------------
        public RemuneracionesController()
        {
            _listarRemu = new ListarRemuneracionesLN();
            _datosPersonalesLN = new DatosPersonalesLN();
            _crearRemuneracionesLN = new CrearRemuneracionesLN();
            _listarTipoRemuneracionLN = new ListarTipoRemuneracionLN();
            _obtenerEmpleadoPorIdLN = new ObtenerEmpleadoPorIdLN();
            _eliminarRemuneracionLN = new EliminarRemuneracionLN();
            _editarRemuneracionLN = new EditarRemuneracionLN();
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

            var empleado = _datosPersonalesLN.ObtenerEmpleadoPorId(idEmpleado);

            if (empleado == null)
            {
                return HttpNotFound();
            }

            var variables = new Tuple
                <EmpleadoDto, List<RemuneracionDto>>
                (empleado,_listarRemu.Listar(idEmpleado));

            ViewBag.SeccionRemu = seccion;
            return View("DetallesRemu", variables);

        }



        // AGREGAR REMUNERACIÓN ------------------------------------------
        private SelectList ObtenerTipoRemuneracionSelectList(int? selectedValue = null)
        {
            var tipoRemuneracion = _listarTipoRemuneracionLN.ObtenerTipoRemuneracion();
            return new SelectList(tipoRemuneracion, "Id", "nombreTipoRemuneracion", selectedValue);
        }

        // GET: Remuneraciones/Create
        [HttpGet]
        public ActionResult _CrearRemuneracionManual(int idEmpleado)
        {
            var nuevoDto = new RemuneracionDto
            {
                idEmpleado = idEmpleado,
                fechaRemuneracion = DateTime.Now,
                idEstado = 1
            };
            ViewBag.TiposRemuneracion = ObtenerTipoRemuneracionSelectList();
            return PartialView("_CrearRemuneracionManual", nuevoDto);
        }

        // POST: Remuneraciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _CrearRemuneracionManual(RemuneracionDto remuneracionDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Asegurarnos que el idEmpleado no se pierda
                    remuneracionDto.fechaRemuneracion = DateTime.Now;
                    remuneracionDto.idEstado = 1;

                    var resultado = await _crearRemuneracionesLN.AgregarRemuneracionManual(remuneracionDto);

                    if (resultado > 0)
                    {
                        return Json(new { success = true, message = "Remuneración creada exitosamente" });
                    }
                    return Json(new { success = false, message = "No se pudo crear la remuneración" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = $"Error: {ex.Message}" });
                }
            }

            ViewBag.TiposRemuneracion = ObtenerTipoRemuneracionSelectList(remuneracionDto.idTipoRemuneracion);

            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage)
                                        .ToList();

            return Json(new { success = false, message = "Error de validación", errors });
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarRemuneracion(int id, int idEmpleado)
        {
            try
            {
                bool resultado = _eliminarRemuneracionLN.EliminarRemuneracion(id);

                if (resultado)
                {
                    TempData["Mensaje"] = "Remuneración eliminada correctamente";
                    TempData["TipoMensaje"] = "success";
                }
                else
                {
                    TempData["Mensaje"] = "Error al eliminar la remuneración";
                    TempData["TipoMensaje"] = "danger";
                }

                return RedirectToAction("DetallesRemu", new { id = idEmpleado });
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = $"Error: {ex.Message}";
                TempData["TipoMensaje"] = "danger";
                return RedirectToAction("DetallesRemu", new { id = idEmpleado });
            }
        }

        // DETALLES DE REMUNERACIÓN ---------------------------------------
        [HttpGet]
        public ActionResult DetallesDeRemuneracion(int id)
        {
            var remuneracion = _listarRemu.ObtenerPorId(id);
            if (remuneracion == null)
            {
                return HttpNotFound();
            }
            return View(remuneracion);
        }
    }
}
