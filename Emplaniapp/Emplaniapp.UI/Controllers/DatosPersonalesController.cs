using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado;
using Emplaniapp.LogicaDeNegocio.Empleado;

namespace Emplaniapp.UI.Controllers
{
    public class DatosPersonalesController : Controller
    {
        // Hacer un layout parcial según figma (propuesta 2)

        // 1. Datos Personales
        // Cambiar datos
        // Verificar Contraseña
        // agregar datos -> admin 
        // activar/desactivar - admin

        // Servicios
        private IEmpleadoLN _empleadoLN;

        public DatosPersonalesController()
        {
            _empleadoLN = new EmpleadoLN();
        }

        // GET: DatosPersonales
        public ActionResult Index()
        {
            try
            {
                var empleados = _empleadoLN.ListarTodos();
                return View(empleados);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar los empleados: " + ex.Message;
                return View(new List<EmpleadoDto>());
            }
        }

        // GET: DatosPersonales/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var empleado = _empleadoLN.ObtenerPorId(id);
                if (empleado == null)
                {
                    return HttpNotFound();
                }
                return View(empleado);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al obtener los detalles del empleado: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: DatosPersonales/Create
        public ActionResult Create()
        {
            try
            {
                // Cargar listas para dropdowns
                ViewBag.Cargos = ObtenerCargos();
                ViewBag.TiposMoneda = ObtenerTiposMoneda();
                ViewBag.Bancos = ObtenerBancos();
                ViewBag.PeriocidadesPago = ObtenerPeriocidadesPago();
                ViewBag.Estados = ObtenerEstados();

                return View(new EmpleadoDto());
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar la página de creación: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: DatosPersonales/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmpleadoDto empleado)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Valores por defecto para nuevo empleado
                    empleado.idDireccion = 1; // Valor temporal hasta implementar direcciones
                    empleado.idEstado = 1; // Activo por defecto

                    // Validaciones adicionales antes de insertar
                    if (empleado.idCargo <= 0)
                        empleado.idCargo = 1; // Valor por defecto

                    if (empleado.idMoneda <= 0)
                        empleado.idMoneda = 1; // Valor por defecto

                    if (empleado.idBanco <= 0)
                        empleado.idBanco = 1; // Valor por defecto

                    // Validar salarios - establecer valores por defecto si están en 0
                    if (empleado.salarioDiario <= 0)
                        empleado.salarioDiario = 50; // Salario mínimo por defecto

                    if (empleado.salarioAprobado <= 0)
                        empleado.salarioAprobado = empleado.salarioDiario * 30; // Aproximado mensual

                    if (empleado.salarioPorMinuto <= 0)
                        empleado.salarioPorMinuto = empleado.salarioDiario / 480; // 8 horas = 480 minutos

                    if (empleado.salarioPoHora <= 0)
                        empleado.salarioPoHora = empleado.salarioDiario / 8; // 8 horas por día

                    if (empleado.salarioPorHoraExtra <= 0)
                        empleado.salarioPorHoraExtra = empleado.salarioPoHora * 1.5; // 50% extra

                    // Validar periocidad de pago
                    if (string.IsNullOrEmpty(empleado.periocidadPago))
                        empleado.periocidadPago = "Mensual";

                    // Log de depuración para ver los valores antes de insertar
                    System.Diagnostics.Debug.WriteLine($"=== CREAR EMPLEADO - VALORES FINALES ===");
                    System.Diagnostics.Debug.WriteLine($"nombre: {empleado.nombre}");
                    System.Diagnostics.Debug.WriteLine($"cedula: {empleado.cedula}");
                    System.Diagnostics.Debug.WriteLine($"correoInstitucional: {empleado.correoInstitucional}");
                    System.Diagnostics.Debug.WriteLine($"idCargo: {empleado.idCargo}");
                    System.Diagnostics.Debug.WriteLine($"idMoneda: {empleado.idMoneda}");
                    System.Diagnostics.Debug.WriteLine($"idBanco: {empleado.idBanco}");
                    System.Diagnostics.Debug.WriteLine($"idEstado: {empleado.idEstado}");
                    System.Diagnostics.Debug.WriteLine($"idDireccion: {empleado.idDireccion}");
                    System.Diagnostics.Debug.WriteLine($"salarioAprobado: {empleado.salarioAprobado}");
                    System.Diagnostics.Debug.WriteLine($"=========================================");

                    bool resultado = _empleadoLN.Insertar(empleado);
                    
                    if (resultado)
                    {
                        TempData["Mensaje"] = "Empleado creado exitosamente";
                        return RedirectToAction("Index", "HojaResumen");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error al guardar el empleado - el método retornó false");
                    }
                }
                else
                {
                    // Log de errores de validación del modelo
                    System.Diagnostics.Debug.WriteLine("=== ERRORES DE VALIDACIÓN ===");
                    foreach (var modelError in ModelState)
                    {
                        foreach (var error in modelError.Value.Errors)
                        {
                            System.Diagnostics.Debug.WriteLine($"{modelError.Key}: {error.ErrorMessage}");
                        }
                    }
                    System.Diagnostics.Debug.WriteLine("=============================");
                }

                // Si llegamos aquí, algo falló, recargamos los datos para el formulario
                ViewBag.Cargos = ObtenerCargos();
                ViewBag.TiposMoneda = ObtenerTiposMoneda();
                ViewBag.Bancos = ObtenerBancos();
                ViewBag.PeriocidadesPago = ObtenerPeriocidadesPago();
                ViewBag.Estados = ObtenerEstados();
                
                return View(empleado);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR EN CREATE: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"INNER EXCEPTION: {ex.InnerException.Message}");
                }

                ModelState.AddModelError("", "Error al crear empleado: " + ex.Message);
                ViewBag.Cargos = ObtenerCargos();
                ViewBag.TiposMoneda = ObtenerTiposMoneda();
                ViewBag.Bancos = ObtenerBancos();
                ViewBag.PeriocidadesPago = ObtenerPeriocidadesPago();
                ViewBag.Estados = ObtenerEstados();
                return View(empleado);
            }
        }

        // GET: DatosPersonales/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var empleado = _empleadoLN.ObtenerPorId(id);
                if (empleado == null)
                {
                    return HttpNotFound();
                }

                // Cargar listas para dropdowns
                ViewBag.Cargos = ObtenerCargos();
                ViewBag.TiposMoneda = ObtenerTiposMoneda();
                ViewBag.Bancos = ObtenerBancos();
                ViewBag.PeriocidadesPago = ObtenerPeriocidadesPago();
                ViewBag.Estados = ObtenerEstados();

                return View(empleado);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar los datos del empleado: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: DatosPersonales/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EmpleadoDto empleado)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    empleado.idEmpleado = id;
                    bool resultado = _empleadoLN.Actualizar(empleado);
                    
                    if (resultado)
                    {
                        TempData["Mensaje"] = "Empleado actualizado exitosamente";
                        return RedirectToAction("Detalles", new { id = id });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error al actualizar el empleado");
                    }
                }

                // Si llegamos aquí, algo falló, recargamos los datos para el formulario
                ViewBag.Cargos = ObtenerCargos();
                ViewBag.TiposMoneda = ObtenerTiposMoneda();
                ViewBag.Bancos = ObtenerBancos();
                ViewBag.PeriocidadesPago = ObtenerPeriocidadesPago();
                ViewBag.Estados = ObtenerEstados();
                
                return View(empleado);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al actualizar empleado: " + ex.Message);
                ViewBag.Cargos = ObtenerCargos();
                ViewBag.TiposMoneda = ObtenerTiposMoneda();
                ViewBag.Bancos = ObtenerBancos();
                ViewBag.PeriocidadesPago = ObtenerPeriocidadesPago();
                ViewBag.Estados = ObtenerEstados();
                return View(empleado);
            }
        }

        // GET: DatosPersonales/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var empleado = _empleadoLN.ObtenerPorId(id);
                if (empleado == null)
                {
                    return HttpNotFound();
                }
                return View(empleado);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al obtener los datos del empleado: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: DatosPersonales/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                bool resultado = _empleadoLN.Eliminar(id);
                
                if (resultado)
                {
                    TempData["Mensaje"] = "Empleado eliminado exitosamente";
                }
                else
                {
                    TempData["Error"] = "Error al eliminar el empleado";
                }
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al eliminar empleado: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: DatosPersonales/Detalles/5
        public ActionResult Detalles(int id)
        {
            try
            {
                var empleado = _empleadoLN.ObtenerPorId(id);
                
                if (empleado == null)
                {
                    return HttpNotFound();
                }

                return View(empleado);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al obtener los detalles del empleado: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: DatosPersonales/Historial/5
        public ActionResult Historial(int id)
        {
            try
            {
                var empleado = _empleadoLN.ObtenerPorId(id);
                
                if (empleado == null)
                {
                    return HttpNotFound();
                }

                ViewBag.Seccion = "Historial";
                return View("Detalles", empleado);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al obtener el historial del empleado: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: DatosPersonales/Remuneraciones/5
        public ActionResult Remuneraciones(int id)
        {
            try
            {
                var empleado = _empleadoLN.ObtenerPorId(id);
                
                if (empleado == null)
                {
                    return HttpNotFound();
                }

                ViewBag.Seccion = "Remuneraciones";
                return View("Detalles", empleado);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al obtener las remuneraciones del empleado: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: DatosPersonales/Retenciones/5
        public ActionResult Retenciones(int id)
        {
            try
            {
                var empleado = _empleadoLN.ObtenerPorId(id);
                
                if (empleado == null)
                {
                    return HttpNotFound();
                }

                ViewBag.Seccion = "Retenciones";
                return View("Detalles", empleado);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al obtener las retenciones del empleado: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: DatosPersonales/Observaciones/5
        public ActionResult Observaciones(int id)
        {
            try
            {
                var empleado = _empleadoLN.ObtenerPorId(id);
                
                if (empleado == null)
                {
                    return HttpNotFound();
                }

                ViewBag.Seccion = "Observaciones";
                return View("Detalles", empleado);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al obtener las observaciones del empleado: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: DatosPersonales/Liquidacion/5
        public ActionResult Liquidacion(int id)
        {
            try
            {
                var empleado = _empleadoLN.ObtenerPorId(id);
                
                if (empleado == null)
                {
                    return HttpNotFound();
                }

                ViewBag.Seccion = "Liquidación";
                return View("Detalles", empleado);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al obtener la liquidación del empleado: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: DatosPersonales/EditarDatosPersonales/5
        public ActionResult EditarDatosPersonales(int id)
        {
            try
            {
                var empleado = _empleadoLN.ObtenerPorId(id);
                
                if (empleado == null)
                {
                    return HttpNotFound();
                }
                
                // Crear un modelo para la edición de datos personales
                var datosPersonales = new DatosPersonalesViewModel
                {
                    IdEmpleado = empleado.idEmpleado,
                    Nombre = empleado.nombre,
                    SegundoNombre = empleado.segundoNombre,
                    PrimerApellido = empleado.primerApellido,
                    SegundoApellido = empleado.segundoApellido,
                    FechaNacimiento = empleado.fechaNacimiento,
                    Cedula = empleado.cedula,
                    NumeroTelefonico = empleado.numeroTelefonico,
                    CorreoInstitucional = empleado.correoInstitucional
                };
                
                return View(datosPersonales);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar los datos personales: " + ex.Message;
                return RedirectToAction("Detalles", new { id = id });
            }
        }

        // POST: DatosPersonales/EditarDatosPersonales/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarDatosPersonales(DatosPersonalesViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Obtener el empleado actual
                    var empleado = _empleadoLN.ObtenerPorId(model.IdEmpleado);
                    if (empleado != null)
                    {
                        // Actualizar solo los campos personales
                        empleado.nombre = model.Nombre;
                        empleado.segundoNombre = model.SegundoNombre;
                        empleado.primerApellido = model.PrimerApellido;
                        empleado.segundoApellido = model.SegundoApellido;
                        empleado.fechaNacimiento = model.FechaNacimiento;
                        empleado.cedula = model.Cedula;
                        empleado.numeroTelefonico = model.NumeroTelefonico;
                        empleado.correoInstitucional = model.CorreoInstitucional;

                        bool resultado = _empleadoLN.Actualizar(empleado);
                        
                        if (resultado)
                        {
                            TempData["Mensaje"] = "Datos personales actualizados correctamente";
                            return RedirectToAction("Detalles", new { id = model.IdEmpleado });
                        }
                        else
                        {
                            ModelState.AddModelError("", "Error al guardar los cambios");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "No se encontró el empleado");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al guardar los cambios: " + ex.Message);
                }
            }
            
            return View(model);
        }

        // GET: DatosPersonales/EditarDatosLaborales/5
        public ActionResult EditarDatosLaborales(int id)
        {
            try
            {
                var empleado = _empleadoLN.ObtenerPorId(id);
                
                if (empleado == null)
                {
                    return HttpNotFound();
                }
                
                // Crear un modelo para la edición de datos laborales
                var datosLaborales = new DatosLaboralesViewModel
                {
                    IdEmpleado = empleado.idEmpleado,
                    IdCargo = empleado.idCargo,
                    FechaIngreso = empleado.fechaContratacion,
                    FechaSalida = empleado.fechaSalida
                };
                
                ViewBag.Cargos = ObtenerCargos();
                return View(datosLaborales);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar los datos laborales: " + ex.Message;
                return RedirectToAction("Detalles", new { id = id });
            }
        }

        // POST: DatosPersonales/EditarDatosLaborales/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarDatosLaborales(DatosLaboralesViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Obtener el empleado actual
                    var empleado = _empleadoLN.ObtenerPorId(model.IdEmpleado);
                    if (empleado != null)
                    {
                        // Actualizar solo los campos laborales
                        empleado.idCargo = model.IdCargo;
                        empleado.fechaContratacion = model.FechaIngreso;
                        empleado.fechaSalida = model.FechaSalida;

                        bool resultado = _empleadoLN.Actualizar(empleado);
                        
                        if (resultado)
                        {
                            TempData["Mensaje"] = "Datos laborales actualizados correctamente";
                            return RedirectToAction("Detalles", new { id = model.IdEmpleado });
                        }
                        else
                        {
                            ModelState.AddModelError("", "Error al guardar los cambios");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "No se encontró el empleado");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al guardar los cambios: " + ex.Message);
                }
            }
            
            // Si llegamos aquí, algo falló, recargamos los datos para el formulario
            ViewBag.Cargos = ObtenerCargos();
            return View(model);
        }

        // GET: DatosPersonales/EditarDatosFinancieros/5
        public ActionResult EditarDatosFinancieros(int id)
        {
            try
            {
                var empleado = _empleadoLN.ObtenerPorId(id);
                
                if (empleado == null)
                {
                    return HttpNotFound();
                }
                
                // Crear un modelo para la edición de datos financieros
                var datosFinancieros = new DatosFinancierosViewModel
                {
                    IdEmpleado = empleado.idEmpleado,
                    PeriocidadPago = empleado.periocidadPago,
                    SalarioAprobado = empleado.salarioAprobado,
                    SalarioDiario = empleado.salarioDiario,
                    IdTipoMoneda = empleado.idMoneda,
                    CuentaIBAN = empleado.cuentaIBAN,
                    IdBanco = empleado.idBanco
                };
                
                ViewBag.TiposMoneda = ObtenerTiposMoneda();
                ViewBag.Bancos = ObtenerBancos();
                ViewBag.PeriocidadesPago = ObtenerPeriocidadesPago();
                
                return View(datosFinancieros);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar los datos financieros: " + ex.Message;
                return RedirectToAction("Detalles", new { id = id });
            }
        }

        // POST: DatosPersonales/EditarDatosFinancieros/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarDatosFinancieros(DatosFinancierosViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Obtener el empleado actual
                    var empleado = _empleadoLN.ObtenerPorId(model.IdEmpleado);
                    if (empleado != null)
                    {
                        // Actualizar los campos financieros usando IDs directamente
                        empleado.periocidadPago = model.PeriocidadPago;
                        empleado.salarioAprobado = model.SalarioAprobado;
                        empleado.salarioDiario = model.SalarioDiario;
                        empleado.cuentaIBAN = model.CuentaIBAN;
                        empleado.idMoneda = model.IdTipoMoneda;
                        empleado.idBanco = model.IdBanco;

                        bool resultado = _empleadoLN.Actualizar(empleado);
                        
                        if (resultado)
                        {
                            TempData["Mensaje"] = "Datos financieros actualizados correctamente";
                            return RedirectToAction("Detalles", new { id = model.IdEmpleado });
                        }
                        else
                        {
                            ModelState.AddModelError("", "Error al guardar los cambios");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "No se encontró el empleado");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al guardar los cambios: " + ex.Message);
                }
            }
            
            // Recargar las listas si hay errores
            ViewBag.TiposMoneda = ObtenerTiposMoneda();
            ViewBag.Bancos = ObtenerBancos();
            ViewBag.PeriocidadesPago = ObtenerPeriocidadesPago();
            return View(model);
        }

        #region Métodos Auxiliares
        
        // Método para obtener la lista de cargos desde la base de datos
        private List<SelectListItem> ObtenerCargos()
        {
            try
            {
                var cargos = _empleadoLN.ObtenerCargos();
                var lista = cargos.Select(c => new SelectListItem 
                { 
                    Value = c.idCargo.ToString(), 
                    Text = c.nombreCargo 
                }).ToList();
                
                // Si no hay cargos, agregar uno por defecto
                if (!lista.Any())
                {
                    lista.Add(new SelectListItem { Value = "1", Text = "Cargo General" });
                }
                
                return lista;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al obtener cargos: {ex.Message}");
                // Devolver una lista con un elemento por defecto
                return new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Cargo General" }
                };
            }
        }
        
        // Método para obtener tipos de moneda desde la base de datos
        private List<SelectListItem> ObtenerTiposMoneda()
        {
            try
            {
                var monedas = _empleadoLN.ObtenerTiposMoneda();
                var lista = monedas.Select(m => new SelectListItem 
                { 
                    Value = m.idMoneda.ToString(), 
                    Text = m.nombreMoneda 
                }).ToList();
                
                // Si no hay monedas, agregar una por defecto
                if (!lista.Any())
                {
                    lista.Add(new SelectListItem { Value = "1", Text = "USD - Dólar Americano" });
                }
                
                return lista;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al obtener tipos de moneda: {ex.Message}");
                // Devolver una lista con un elemento por defecto
                return new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "USD - Dólar Americano" }
                };
            }
        }
        
        // Método para obtener bancos desde la base de datos
        private List<SelectListItem> ObtenerBancos()
        {
            try
            {
                var bancos = _empleadoLN.ObtenerBancos();
                var lista = bancos.Select(b => new SelectListItem 
                { 
                    Value = b.idBanco.ToString(), 
                    Text = b.nombreBanco 
                }).ToList();
                
                // Si no hay bancos, agregar uno por defecto
                if (!lista.Any())
                {
                    lista.Add(new SelectListItem { Value = "1", Text = "Banco General" });
                }
                
                return lista;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al obtener bancos: {ex.Message}");
                // Devolver una lista con un elemento por defecto
                return new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Banco General" }
                };
            }
        }

        // Método para obtener estados desde la base de datos
        private List<SelectListItem> ObtenerEstados()
        {
            try
            {
                var estados = _empleadoLN.ObtenerEstados();
                var lista = estados.Select(e => new SelectListItem 
                { 
                    Value = e.idEstado.ToString(), 
                    Text = e.nombreEstado 
                }).ToList();
                
                // Si no hay estados, agregar uno por defecto
                if (!lista.Any())
                {
                    lista.Add(new SelectListItem { Value = "1", Text = "Activo" });
                }
                
                return lista;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al obtener estados: {ex.Message}");
                // Devolver una lista con un elemento por defecto
                return new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Activo" }
                };
            }
        }
        
        // Método para obtener periocidades de pago (estático)
        private List<SelectListItem> ObtenerPeriocidadesPago()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "Quincenal", Text = "Quincenal" },
                new SelectListItem { Value = "Mensual", Text = "Mensual" },
                new SelectListItem { Value = "Semanal", Text = "Semanal" }
            };
        }

        #endregion
    }

    // ViewModel para la edición de datos laborales
    public class DatosLaboralesViewModel
    {
        public int IdEmpleado { get; set; }
        public int IdCargo { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime? FechaSalida { get; set; }
    }

    // ViewModel para la edición de datos financieros
    public class DatosFinancierosViewModel
    {
        public int IdEmpleado { get; set; }
        public string PeriocidadPago { get; set; }
        public double SalarioAprobado { get; set; }
        public double SalarioDiario { get; set; }
        public int IdTipoMoneda { get; set; }
        public string CuentaIBAN { get; set; }
        public int IdBanco { get; set; }
    }

    // ViewModel para la edición de datos personales
    public class DatosPersonalesViewModel
    {
        public int IdEmpleado { get; set; }
        public string Nombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int Cedula { get; set; }
        public string NumeroTelefonico { get; set; }
        public string CorreoInstitucional { get; set; }
    }
}
