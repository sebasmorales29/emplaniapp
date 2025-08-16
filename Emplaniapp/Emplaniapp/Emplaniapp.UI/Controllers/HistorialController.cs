using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Emplaniapp.Abstracciones.InterfacesParaUI.Historial;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.LogicaDeNegocio.Historial;
using Emplaniapp.AccesoADatos;
using Microsoft.AspNet.Identity;
using Emplaniapp.UI.Attributes;

namespace Emplaniapp.UI.Controllers
{
    [Authorize]
    public class HistorialController : Controller
    {
        private IConsultarHistorialLN _consultarHistorialLN;
        private IRegistrarEventoHistorialLN _registrarEventoHistorialLN;

        public HistorialController()
        {
            _consultarHistorialLN = new ConsultarHistorialLN();
            _registrarEventoHistorialLN = new RegistrarEventoHistorialLN();
        }

        // GET: Historial/Test
        public ActionResult Test()
        {
            return Content("Controlador de Historial funcionando correctamente");
        }

        // GET: Historial/TestData
        public ActionResult TestData()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("🧪 Ejecutando TestData...");
                
                // Devolver contenido simple para probar
                return Content("Test de historial funcionando correctamente - Empleado ID: " + Request.QueryString["id"]);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en TestData: {ex.Message}");
                return Content($"Error en TestData: {ex.Message}");
            }
        }

        // GET: Historial/TestSimple
        public ActionResult TestSimple()
        {
            return Content("Test Simple OK");
        }

        // GET: Historial/TestBasic
        public ActionResult TestBasic()
        {
            try
            {
                // Acción completamente básica sin dependencias
                return Json(new { success = true, message = "Controlador funcionando", timestamp = DateTime.Now }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Historial/Empleado/{id}
        public ActionResult Empleado(int id)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"🚀 Iniciando acción Empleado con ID: {id}");
                
                if (id <= 0)
                {
                    System.Diagnostics.Debug.WriteLine("❌ ID de empleado inválido");
                    TempData["Error"] = "ID de empleado inválido";
                    return RedirectToAction("Index", "Empleado");
                }

                System.Diagnostics.Debug.WriteLine($"🔍 Obteniendo historial para empleado {id}...");
                
                // Obtener historial reciente del empleado
                var historialReciente = _consultarHistorialLN.ObtenerHistorialReciente(id, 20);
                System.Diagnostics.Debug.WriteLine($"📊 Historial obtenido: {historialReciente?.Count ?? 0} eventos");
                
                        // Si no hay eventos, mostrar lista vacía
        if (historialReciente == null || !historialReciente.Any())
        {
            historialReciente = new List<HistorialEmpleadoDto>();
            System.Diagnostics.Debug.WriteLine($"📭 No hay eventos de historial para mostrar");
        }
                
                var totalEventos = historialReciente.Count;
                System.Diagnostics.Debug.WriteLine($"📊 Total eventos: {totalEventos}");
                
                var categorias = historialReciente.Select(e => e.categoriaEvento).Distinct().ToList();
                System.Diagnostics.Debug.WriteLine($"📊 Categorías obtenidas: {string.Join(", ", categorias ?? new List<string>())}");

                ViewBag.TotalEventos = totalEventos;
                ViewBag.Categorias = categorias;
                ViewBag.IdEmpleado = id;

                // Si es una petición AJAX, devolver solo el contenido
                if (Request.IsAjaxRequest() || Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    System.Diagnostics.Debug.WriteLine($"✅ Petición AJAX detectada para empleado {id}");
                    System.Diagnostics.Debug.WriteLine($"📊 Historial obtenido: {historialReciente?.Count ?? 0} eventos");
                    System.Diagnostics.Debug.WriteLine($"📊 Total eventos: {totalEventos}");
                    System.Diagnostics.Debug.WriteLine($"📊 Categorías: {string.Join(", ", categorias ?? new List<string>())}");
                    
                    // Asegurar que el ViewBag esté disponible en el partial view
                    ViewBag.TotalEventos = totalEventos;
                    ViewBag.Categorias = categorias;
                    ViewBag.IdEmpleado = id;
                    ViewBag.Debug = true; // Activar debug temporalmente
                    
                    return PartialView("_HistorialSimple", historialReciente);
                }

                System.Diagnostics.Debug.WriteLine($"✅ Petición normal detectada para empleado {id}");
                return View("EmpleadoCompleto", historialReciente);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en Historial/Empleado: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"❌ Stack Trace: {ex.StackTrace}");
                
                if (Request.IsAjaxRequest() || Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Content($"Error al cargar el historial: {ex.Message}");
                }
                
                TempData["Error"] = "Error al cargar el historial del empleado";
                return RedirectToAction("Index", "Empleado");
            }
        }

        // GET: Historial/VerificarEventos
        [HttpGet]
        public ActionResult VerificarEventos()
        {
            try
            {
                using (var contexto = new Emplaniapp.AccesoADatos.Contexto())
                {
                    var eventos = contexto.TiposEventoHistorial
                        .Where(t => t.idEstado == 1)
                        .Select(t => new { t.nombreEvento, t.categoriaEvento, t.iconoEvento, t.colorEvento })
                        .ToList();
                    
                    var resultado = new
                    {
                        total = eventos.Count,
                        eventos = eventos
                    };
                    
                    return Json(resultado, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Historial/Filtrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filtrar(int idEmpleado, string categoriaEvento = null, DateTime? fechaInicio = null, DateTime? fechaFin = null, int top = 100)
        {
            try
            {
                if (idEmpleado <= 0)
                {
                    return Json(new { success = false, message = "ID de empleado inválido" });
                }

                var historial = _consultarHistorialLN.ObtenerHistorialEmpleado(
                    idEmpleado, 
                    null, 
                    fechaInicio, 
                    fechaFin, 
                    categoriaEvento, 
                    top
                );

                return Json(new { success = true, data = historial });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en Filtrar: {ex.Message}");
                return Json(new { success = false, message = "Error al filtrar el historial" });
            }
        }

        // POST: Historial/PorCategoria
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PorCategoria(int idEmpleado, string categoriaEvento, int top = 100)
        {
            try
            {
                if (idEmpleado <= 0)
                {
                    return Json(new { success = false, message = "ID de empleado inválido" });
                }

                if (string.IsNullOrWhiteSpace(categoriaEvento))
                {
                    return Json(new { success = false, message = "Categoría no especificada" });
                }

                var historial = _consultarHistorialLN.ObtenerHistorialPorCategoria(idEmpleado, categoriaEvento, top);

                return Json(new { success = true, data = historial });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en PorCategoria: {ex.Message}");
                return Json(new { success = false, message = "Error al obtener historial por categoría" });
            }
        }

        // POST: Historial/PorPeriodo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PorPeriodo(int idEmpleado, string periodo, int top = 100)
        {
            try
            {
                if (idEmpleado <= 0)
                {
                    return Json(new { success = false, message = "ID de empleado inválido" });
                }

                if (string.IsNullOrWhiteSpace(periodo))
                {
                    periodo = "mes";
                }

                var historial = _consultarHistorialLN.ObtenerHistorialPorPeriodo(idEmpleado, periodo);

                return Json(new { success = true, data = historial });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en PorPeriodo: {ex.Message}");
                return Json(new { success = false, message = "Error al obtener historial por período" });
            }
        }

        // POST: Historial/RegistrarEvento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarEvento(int idEmpleado, string nombreEvento, string descripcionEvento, string detallesEvento = null, string valorAnterior = null, string valorNuevo = null)
        {
            try
            {
                if (idEmpleado <= 0)
                {
                    return Json(new { success = false, message = "ID de empleado inválido" });
                }

                if (string.IsNullOrWhiteSpace(nombreEvento) || string.IsNullOrWhiteSpace(descripcionEvento))
                {
                    return Json(new { success = false, message = "Nombre y descripción del evento son obligatorios" });
                }

                var resultado = _registrarEventoHistorialLN.RegistrarEvento(
                    idEmpleado, 
                    nombreEvento, 
                    descripcionEvento, 
                    detallesEvento, 
                    valorAnterior, 
                    valorNuevo
                );

                if (resultado)
                {
                    return Json(new { success = true, message = "Evento registrado exitosamente" });
                }
                else
                {
                    return Json(new { success = false, message = "Error al registrar el evento" });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en RegistrarEvento: {ex.Message}");
                return Json(new { success = false, message = "Error interno al registrar el evento" });
            }
        }

        // GET: Historial/Exportar/{id}
        public ActionResult Exportar(int id, string formato = "pdf")
        {
            try
            {
                if (id <= 0)
                {
                    TempData["Error"] = "ID de empleado inválido";
                    return RedirectToAction("Index", "Empleado");
                }

                // Obtener todo el historial del empleado
                var historial = _consultarHistorialLN.ObtenerHistorialEmpleado(id, null, null, null, null, 1000);

                if (formato.ToLower() == "excel")
                {
                    // Implementar exportación a Excel
                    return ExportarAExcel(historial, id);
                }
                else
                {
                    // Implementar exportación a PDF
                    return ExportarAPDF(historial, id);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en Exportar: {ex.Message}");
                TempData["Error"] = "Error al exportar el historial";
                return RedirectToAction("Empleado", new { id });
            }
        }

        private ActionResult ExportarAExcel(List<HistorialEmpleadoDto> historial, int idEmpleado)
        {
            // TODO: Implementar exportación a Excel
            TempData["Info"] = "Exportación a Excel en desarrollo";
            return RedirectToAction("Empleado", new { id = idEmpleado });
        }

        private ActionResult ExportarAPDF(List<HistorialEmpleadoDto> historial, int idEmpleado)
        {
            // TODO: Implementar exportación a PDF
            TempData["Info"] = "Exportación a PDF en desarrollo";
            return RedirectToAction("Empleado", new { id = idEmpleado });
        }
    }
}
