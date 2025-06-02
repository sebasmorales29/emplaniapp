/**
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Emplaniapp.Abstracciones.ModelosParaUI;

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


        // GET: DatosPersonales
        public ActionResult Index()
        {
            return View();
        }

        // GET: DatosPersonales/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DatosPersonales/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DatosPersonales/Create
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

        // GET: DatosPersonales/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DatosPersonales/Edit/5
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

        // GET: DatosPersonales/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DatosPersonales/Delete/5
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

        // GET: DatosPersonales/Detalles/5
        public ActionResult Detalles(int id)
        {
            // En un escenario real, aquí obtendríamos los datos del empleado de la base de datos
            // Por ahora, crearemos un empleado de ejemplo similar al de la imagen
            var empleado = ObtenerEmpleadoPorId(id);
            
            if (empleado == null)
            {
                return HttpNotFound();
            }

            return View(empleado);
        }

        // GET: DatosPersonales/Historial/5
        public ActionResult Historial(int id)
        {
            var empleado = ObtenerEmpleadoPorId(id);
            
            if (empleado == null)
            {
                return HttpNotFound();
            }

            ViewBag.Seccion = "Historial";
            return View("Detalles", empleado);
        }

        // GET: DatosPersonales/Remuneraciones/5
        public ActionResult Remuneraciones(int id)
        {
            var empleado = ObtenerEmpleadoPorId(id);
            
            if (empleado == null)
            {
                return HttpNotFound();
            }

            ViewBag.Seccion = "Remuneraciones";
            return View("Detalles", empleado);
        }

        // GET: DatosPersonales/Retenciones/5
        public ActionResult Retenciones(int id)
        {
            var empleado = ObtenerEmpleadoPorId(id);
            
            if (empleado == null)
            {
                return HttpNotFound();
            }

            ViewBag.Seccion = "Retenciones";
            return View("Detalles", empleado);
        }

        // GET: DatosPersonales/Observaciones/5
        public ActionResult Observaciones(int id)
        {
            var empleado = ObtenerEmpleadoPorId(id);
            
            if (empleado == null)
            {
                return HttpNotFound();
            }

            ViewBag.Seccion = "Observaciones";
            return View("Detalles", empleado);
        }

        // GET: DatosPersonales/Liquidacion/5
        public ActionResult Liquidacion(int id)
        {
            var empleado = ObtenerEmpleadoPorId(id);
            
            if (empleado == null)
            {
                return HttpNotFound();
            }

            ViewBag.Seccion = "Liquidación";
            return View("Detalles", empleado);
        }

        // GET: DatosPersonales/EditarDatosLaborales/5
        public ActionResult EditarDatosLaborales(int id)
        {
            var empleado = ObtenerEmpleadoPorId(id);
            
            if (empleado == null)
            {
                return HttpNotFound();
            }
            
            // Crear un modelo para la edición de datos laborales
            var datosLaborales = new DatosLaboralesViewModel
            {
                IdEmpleado = empleado.idEmpleado,
                NumeroOcupacion = "9832", // En un caso real, obtendríamos este dato
                Cargo = empleado.nombreCargo,
                FechaIngreso = empleado.fechaContratacion,
                FechaSalida = empleado.fechaSalida,
                InicioVacaciones = null // En un caso real, obtendríamos este dato
            };
            
            // Aquí cargaríamos listas para dropdowns como cargos disponibles
            ViewBag.Cargos = ObtenerCargos();
            
            return View(datosLaborales);
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
                    // Aquí actualizaríamos los datos en la base de datos
                    // Por ahora solo redirigimos de vuelta a los detalles
                    
                    // Simulamos éxito
                    TempData["Mensaje"] = "Datos laborales actualizados correctamente";
                    return RedirectToAction("Detalles", new { id = model.IdEmpleado });
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
            var empleado = ObtenerEmpleadoPorId(id);
            
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
                TipoMoneda = empleado.nombreMoneda,
                CuentaIBAN = empleado.cuentaIBAN,
                Banco = empleado.nombreBanco
            };
            
            // Aquí cargaríamos listas para dropdowns
            ViewBag.TiposMoneda = ObtenerTiposMoneda();
            ViewBag.Bancos = ObtenerBancos();
            ViewBag.PeriocidadesPago = ObtenerPeriocidadesPago();
            
            return View(datosFinancieros);
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
                    // Aquí actualizaríamos los datos en la base de datos
                    // Por ahora solo redirigimos de vuelta a los detalles
                    
                    // Simulamos éxito
                    TempData["Mensaje"] = "Datos financieros actualizados correctamente";
                    return RedirectToAction("Detalles", new { id = model.IdEmpleado });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al guardar los cambios: " + ex.Message);
                }
            }
            
            // Si llegamos aquí, algo falló, recargamos los datos para el formulario
            ViewBag.TiposMoneda = ObtenerTiposMoneda();
            ViewBag.Bancos = ObtenerBancos();
            ViewBag.PeriocidadesPago = ObtenerPeriocidadesPago();
            return View(model);
        }

        #region Métodos Auxiliares
        
        // Método para obtener un empleado por su ID (simulado por ahora)
        private EmpleadoDto ObtenerEmpleadoPorId(int id)
        {
            // En un escenario real, esto sería una consulta a la base de datos
            // Por ahora, creamos un empleado de ejemplo basado en la imagen
            return new EmpleadoDto
            {
                idEmpleado = id,
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
            };
        }
        
        // Método para obtener la lista de cargos (simulado)
        private List<SelectListItem> ObtenerCargos()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Administrador" },
                new SelectListItem { Value = "2", Text = "Contador" },
                new SelectListItem { Value = "3", Text = "Gerente" },
                new SelectListItem { Value = "4", Text = "Recepcionista" },
                new SelectListItem { Value = "5", Text = "Desarrollador" }
            };
        }
        
        // Método para obtener tipos de moneda (simulado)
        private List<SelectListItem> ObtenerTiposMoneda()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Colones" },
                new SelectListItem { Value = "2", Text = "Dólares" },
                new SelectListItem { Value = "3", Text = "Euros" }
            };
        }
        
        // Método para obtener bancos (simulado)
        private List<SelectListItem> ObtenerBancos()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "BN" },
                new SelectListItem { Value = "2", Text = "BCR" },
                new SelectListItem { Value = "3", Text = "BAC" },
                new SelectListItem { Value = "4", Text = "Scotiabank" }
            };
        }
        
        // Método para obtener periocidades de pago (simulado)
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
        public string NumeroOcupacion { get; set; }
        public string Cargo { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime? FechaSalida { get; set; }
        public DateTime? InicioVacaciones { get; set; }
    }

    // ViewModel para la edición de datos financieros
    public class DatosFinancierosViewModel
    {
        public int IdEmpleado { get; set; }
        public string PeriocidadPago { get; set; }
        public double SalarioAprobado { get; set; }
        public double SalarioDiario { get; set; }
        public string TipoMoneda { get; set; }
        public string CuentaIBAN { get; set; }
        public string Banco { get; set; }
    }
}
**/