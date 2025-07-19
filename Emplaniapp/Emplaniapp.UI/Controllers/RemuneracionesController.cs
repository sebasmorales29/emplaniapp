using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Emplaniapp.Abstracciones.InterfacesAD.Remuneraciones.ObtenerRemuneracionPorId;
using Emplaniapp.Abstracciones.InterfacesParaUI;
using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones;
using Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones.CrearRemuneraciones;
using Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones.EditarRemuneracion;
using Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones.EliminarRemuneracion;
using Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones.ObtenerRemuneracionPorId;
using Emplaniapp.Abstracciones.InterfacesParaUI.Tipo_Remuneracion;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.LogicaDeNegocio;
using Emplaniapp.LogicaDeNegocio.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.LogicaDeNegocio.Remuneraciones;
using Emplaniapp.LogicaDeNegocio.Remuneraciones.CrearRemuneraciones;
using Emplaniapp.LogicaDeNegocio.Remuneraciones.EditarRemuneracion;
using Emplaniapp.LogicaDeNegocio.Remuneraciones.EliminarRemuneracion;
using Emplaniapp.LogicaDeNegocio.Remuneraciones.ObtenerRemuneracionPorId;
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
        private IObtenerRemuneracionPorIdLN _obtenerRemuneracionPorIdLN;
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
            _obtenerRemuneracionPorIdLN = new ObtenerRemuneracionPorIdLN();
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
        public ActionResult CrearRemuneracionManual(int idEmpleado)
        {
            var nuevoDto = new RemuneracionDto
            {
                idEmpleado = idEmpleado,
                fechaRemuneracion = DateTime.Now,
                idEstado = 1
            };

            ViewBag.TiposRemuneracion = ObtenerTipoRemuneracionSelectList();
            return View("CrearRemuneracionManual", nuevoDto);
        }

        // POST: Remuneraciones/CrearRemuneracionManual
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CrearRemuneracionManual(RemuneracionDto remuneracionDto, int idEmpleado)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    remuneracionDto.fechaRemuneracion = DateTime.Now;
                    remuneracionDto.idEstado = 1;

                    var resultado = await _crearRemuneracionesLN.AgregarRemuneracionManual(remuneracionDto);

                    if (resultado > 0)
                    {
                        TempData["mensaje"] = "Remuneración creada exitosamente.";
                        return RedirectToAction("DetallesRemu", new { id = remuneracionDto.idEmpleado });
                    }

                    TempData["mensaje"] = "No se pudo crear la remuneración.";
                    return RedirectToAction("DetallesRemu", new { id = remuneracionDto.idEmpleado });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }

            ViewBag.TiposRemuneracion = ObtenerTipoRemuneracionSelectList(remuneracionDto.idTipoRemuneracion);
            return RedirectToAction("DetallesRemu", new { id = idEmpleado });
        }


        // EDITAR REMUNERACIÓN ------------------------------------------

        // GET: Remuneraciones/Edit/5
        public ActionResult EditarRemuneracion(int id, int idEmpleado)
        {
            var remuneracion = _obtenerRemuneracionPorIdLN.ObtenerPorId(id);

            if (remuneracion == null)
            {
                return HttpNotFound("No se encontró la remuneración.");
            }
            remuneracion.idEmpleado = idEmpleado;

            return View("EditarRemuneracion", remuneracion);
        }


        // POST: Remuneraciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarRemuneracion(RemuneracionDto remuneracion, int idEmpleado)
        {
            try
            {
                if (!ModelState.IsValid)
                            {
                                return View("EditarRemuneracion", remuneracion);
                            }

                            // Cálculo automático si es Horas Extras
                            if (remuneracion.nombreTipoRemuneracion != null &&
                                remuneracion.nombreTipoRemuneracion.Equals("Horas Extra", System.StringComparison.OrdinalIgnoreCase))
                            {
                                remuneracion.pagoQuincenal = remuneracion.horas * 1.5m; // Ajusta el multiplicador si es necesario
                            }

                            int resultado = _editarRemuneracionLN.Actualizar(remuneracion);

                            if (resultado > 0)
                            {
                                TempData["mensaje"] = "Remuneración actualizada correctamente.";
                            }
                            else
                            {
                                TempData["mensaje"] = "No se pudo actualizar la remuneración.";
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
