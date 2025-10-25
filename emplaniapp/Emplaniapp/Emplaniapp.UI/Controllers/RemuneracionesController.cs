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
using Emplaniapp.Abstracciones.InterfacesParaUI.Historial;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.LogicaDeNegocio;
using Microsoft.AspNet.Identity;
using Emplaniapp.LogicaDeNegocio.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.LogicaDeNegocio.Remuneraciones;
using Emplaniapp.LogicaDeNegocio.Historial;
using Emplaniapp.LogicaDeNegocio.Remuneraciones.CrearRemuneraciones;
using Emplaniapp.LogicaDeNegocio.Remuneraciones.EditarRemuneracion;
using Emplaniapp.LogicaDeNegocio.Remuneraciones.EliminarRemuneracion;
using Emplaniapp.LogicaDeNegocio.Remuneraciones.ObtenerRemuneracionPorId;
using Emplaniapp.LogicaDeNegocio.Tipo_Remuneracion;
using Microsoft.AspNet.Identity.Owin;

// >>> AGREGADOS PARA PDF <<<
using Rotativa;
using Rotativa.Options;
// <<< AGREGADOS PARA PDF <<<

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
        private IRegistrarEventoHistorialLN _registrarEventoHistorialLN;
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
            _registrarEventoHistorialLN = new RegistrarEventoHistorialLN();
        }

        // Constructor para inyección de dependencias (usado por Identity/OWIN)
        public RemuneracionesController(ApplicationUserManager userManager)
        {
            _listarRemu = new ListarRemuneracionesLN();
            _datosPersonalesLN = new DatosPersonalesLN();
            _crearRemuneracionesLN = new CrearRemuneracionesLN();
            _listarTipoRemuneracionLN = new ListarTipoRemuneracionLN();
            _obtenerEmpleadoPorIdLN = new ObtenerEmpleadoPorIdLN();
            _eliminarRemuneracionLN = new EliminarRemuneracionLN();
            _editarRemuneracionLN = new EditarRemuneracionLN();
            _obtenerRemuneracionPorIdLN = new ObtenerRemuneracionPorIdLN();
            _registrarEventoHistorialLN = new RegistrarEventoHistorialLN();
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
                (empleado, _listarRemu.Listar(idEmpleado));

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
            var empleado = _obtenerEmpleadoPorIdLN.ObtenerEmpleadoPorId(idEmpleado);

            bool esVendedor = false;

            if (empleado != null && !string.IsNullOrEmpty(empleado.nombreCargo))
            {
                if (empleado.nombreCargo.ToLower().Contains("vendedor"))
                {
                    esVendedor = true;
                }
            }

            ViewBag.EsVendedor = esVendedor;

            // Obtener todas las opciones de tipos remuneracion
            var tiposRemuneracion = _listarTipoRemuneracionLN.ObtenerTipoRemuneracion();

            if (esVendedor)
            {
                // Filtrar solo la opción con Id = 6 (Pago Quincenal)
                tiposRemuneracion = tiposRemuneracion.Where(t => t.Id == 6).ToList();
            }

            // Crear SelectList con la lista filtrada o completa
            ViewBag.TiposRemuneracion = new SelectList(tiposRemuneracion, "Id", "nombreTipoRemuneracion");

            var nuevoDto = new RemuneracionDto
            {
                idEmpleado = idEmpleado,
                fechaRemuneracion = DateTime.Now,
                idEstado = 1
            };

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

            var empleado = _obtenerEmpleadoPorIdLN.ObtenerEmpleadoPorId(idEmpleado);
            bool esVendedor = false;
            if (empleado != null && !string.IsNullOrEmpty(empleado.nombreCargo))
            {
                if (empleado.nombreCargo.ToLower().Contains("vendedor"))
                {
                    esVendedor = true;
                }
            }
            ViewBag.EsVendedor = esVendedor;

            var tiposRemuneracion = _listarTipoRemuneracionLN.ObtenerTipoRemuneracion();

            if (esVendedor)
            {
                // Filtrar solo la opción con Id = 6 (Pago Quincenal)
                tiposRemuneracion = tiposRemuneracion.Where(t => t.Id == 6).ToList();
            }

            // Crear SelectList con la lista filtrada o completa
            ViewBag.TiposRemuneracion = new SelectList(tiposRemuneracion, "Id", "nombreTipoRemuneracion");
            return View("CrearRemuneracionManual", remuneracionDto);
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
                int resultado = _editarRemuneracionLN.Actualizar(remuneracion);
                if (resultado > 0)
                {
                    // Registrar evento en el historial
                    try
                    {
                        var tipoRemuneracion = _listarTipoRemuneracionLN.ObtenerTipoRemuneracion().FirstOrDefault(t => t.Id == remuneracion.idTipoRemuneracion);

                        _registrarEventoHistorialLN.RegistrarEvento(
                            remuneracion.idEmpleado,
                            "Remuneración Modificada",
                            $"Se modificó la remuneración de {tipoRemuneracion?.nombreTipoRemuneracion ?? "Tipo desconocido"}",
                            $"Monto: {remuneracion.pagoQuincenal:C}",
                            null,
                            remuneracion.pagoQuincenal?.ToString("C") ?? "0.00",
                            User.Identity.GetUserId(),
                            Request.UserHostAddress
                        );
                    }
                    catch (Exception ex)
                    {
                        // Log del error pero no fallar la operación principal
                        System.Diagnostics.Debug.WriteLine($"Error al registrar evento en historial: {ex.Message}");
                    }

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
                    // Registrar evento en el historial
                    try
                    {
                        _registrarEventoHistorialLN.RegistrarEvento(
                            idEmpleado,
                            "Remuneración Eliminada",
                            "Se eliminó una remuneración del empleado",
                            $"ID Remuneración: {id}",
                            null,
                            "Eliminada",
                            User.Identity.GetUserId(),
                            Request.UserHostAddress
                        );
                    }
                    catch (Exception ex)
                    {
                        // Log del error pero no fallar la operación principal
                        System.Diagnostics.Debug.WriteLine($"Error al registrar evento en historial: {ex.Message}");
                    }

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

  
        // GET: Remuneraciones/ReciboPagoPdf/5
        [HttpGet]
        public ActionResult ReciboPagoPdf(int id)
        {
            var remuneracion = _obtenerRemuneracionPorIdLN.ObtenerPorId(id);
            if (remuneracion == null) return HttpNotFound();

            var empleado = _obtenerEmpleadoPorIdLN.ObtenerEmpleadoPorId(remuneracion.idEmpleado);
            var model = new Tuple<EmpleadoDto, RemuneracionDto>(empleado, remuneracion);

            return new ViewAsPdf("ReciboPagoPDF", model)
            {
                FileName = $"Recibo_{empleado.nombre}_{remuneracion.fechaRemuneracion:yyyyMMdd}.pdf",
                PageSize = Size.Letter,
                CustomSwitches = "--print-media-type --enable-local-file-access"
            };
        }
     
    }
}
