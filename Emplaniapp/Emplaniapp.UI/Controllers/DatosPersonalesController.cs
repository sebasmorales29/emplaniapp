using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.Abstracciones.InterfacesParaUI;
using Emplaniapp.LogicaDeNegocio;
using Emplaniapp.UI.Models;

namespace Emplaniapp.UI.Controllers
{
    public class DatosPersonalesController : Controller
    {
        private IDatosPersonalesLN _datosPersonalesLN;

        public DatosPersonalesController()
        {
            _datosPersonalesLN = new DatosPersonalesLN();
        }

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
            var empleado = _datosPersonalesLN.ObtenerEmpleadoPorId(id);
            
            if (empleado == null)
            {
                return HttpNotFound();
            }

            return View(empleado);
        }

        // GET: DatosPersonales/Historial/5
        public ActionResult Historial(int id)
        {
            var empleado = _datosPersonalesLN.ObtenerEmpleadoPorId(id);
            
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
            var empleado = _datosPersonalesLN.ObtenerEmpleadoPorId(id);
            
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
            var empleado = _datosPersonalesLN.ObtenerEmpleadoPorId(id);
            
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
            var empleado = _datosPersonalesLN.ObtenerEmpleadoPorId(id);
            
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
            var empleado = _datosPersonalesLN.ObtenerEmpleadoPorId(id);
            
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
            var empleado = _datosPersonalesLN.ObtenerEmpleadoPorId(id);
            
            if (empleado == null)
            {
                return HttpNotFound();
            }
            
            var datosLaborales = new DatosLaboralesViewModel
            {
                IdEmpleado = empleado.idEmpleado,
                NumeroOcupacion = "9832", // Por ahora hardcodeado
                IdCargo = empleado.idCargo,
                Cargo = empleado.nombreCargo,
                FechaIngreso = empleado.fechaContratacion,
                FechaSalida = empleado.fechaSalida,
                InicioVacaciones = null // Por ahora null
            };
            
            ViewBag.Cargos = ObtenerCargosSelectList(empleado.idCargo);
            
            return View(datosLaborales);
        }

        // POST: DatosPersonales/EditarDatosLaborales/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarDatosLaborales(DatosLaboralesViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool resultado = _datosPersonalesLN.ActualizarDatosLaborales(
                    model.IdEmpleado, 
                    model.IdCargo, 
                    model.FechaIngreso, 
                    model.FechaSalida);

                if (resultado)
                {
                    TempData["Mensaje"] = "Datos laborales actualizados correctamente";
                    TempData["TipoMensaje"] = "success";
                    return RedirectToAction("Detalles", new { id = model.IdEmpleado });
                }
                else
                {
                    ModelState.AddModelError("", "Error al guardar los cambios");
                }
            }
            
            ViewBag.Cargos = ObtenerCargosSelectList(model.IdCargo);
            return View(model);
        }

        // GET: DatosPersonales/EditarDatosFinancieros/5
        public ActionResult EditarDatosFinancieros(int id)
        {
            var empleado = _datosPersonalesLN.ObtenerEmpleadoPorId(id);
            
            if (empleado == null)
            {
                return HttpNotFound();
            }
            
            var datosFinancieros = new DatosFinancierosViewModel
            {
                IdEmpleado = empleado.idEmpleado,
                PeriocidadPago = empleado.periocidadPago,
                SalarioAprobado = empleado.salarioAprobado,
                SalarioDiario = empleado.salarioDiario,
                IdTipoMoneda = empleado.idMoneda,
                TipoMoneda = empleado.nombreMoneda,
                CuentaIBAN = empleado.cuentaIBAN,
                IdBanco = empleado.idBanco,
                Banco = empleado.nombreBanco
            };
            
            ViewBag.TiposMoneda = ObtenerTiposMonedasSelectList(empleado.idMoneda);
            ViewBag.Bancos = ObtenerBancosSelectList(empleado.idBanco);
            ViewBag.PeriocidadesPago = ObtenerPeriocidadesPagoSelectList(empleado.periocidadPago);
            
            return View(datosFinancieros);
        }

        // POST: DatosPersonales/EditarDatosFinancieros/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarDatosFinancieros(DatosFinancierosViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool resultado = _datosPersonalesLN.ActualizarDatosFinancieros(
                    model.IdEmpleado,
                    model.SalarioAprobado,
                    model.SalarioDiario,
                    model.PeriocidadPago,
                    model.IdTipoMoneda,
                    model.CuentaIBAN,
                    model.IdBanco);

                if (resultado)
                {
                    TempData["Mensaje"] = "Datos financieros actualizados correctamente";
                    TempData["TipoMensaje"] = "success";
                    return RedirectToAction("Detalles", new { id = model.IdEmpleado });
                }
                else
                {
                    ModelState.AddModelError("", "Error al guardar los cambios");
                }
            }
            
            ViewBag.TiposMoneda = ObtenerTiposMonedasSelectList(model.IdTipoMoneda);
            ViewBag.Bancos = ObtenerBancosSelectList(model.IdBanco);
            ViewBag.PeriocidadesPago = ObtenerPeriocidadesPagoSelectList(model.PeriocidadPago);
            return View(model);
        }

        #region Métodos Auxiliares
        
        private SelectList ObtenerCargosSelectList(object selectedValue = null)
        {
            var cargos = _datosPersonalesLN.ObtenerCargos()
                .Select(c => new SelectListItem
                {
                    Value = c.idCargo.ToString(),
                    Text = c.nombreCargo
                }).ToList();
            
            return new SelectList(cargos, "Value", "Text", selectedValue);
        }

        private SelectList ObtenerTiposMonedasSelectList(object selectedValue = null)
        {
            var monedas = _datosPersonalesLN.ObtenerTiposMoneda()
                .Select(m => new SelectListItem
                {
                    Value = m.idMoneda.ToString(),
                    Text = m.nombreMoneda
                }).ToList();
            
            return new SelectList(monedas, "Value", "Text", selectedValue);
        }

        private SelectList ObtenerBancosSelectList(object selectedValue = null)
        {
            var bancos = _datosPersonalesLN.ObtenerBancos()
                .Select(b => new SelectListItem
                {
                    Value = b.idBanco.ToString(),
                    Text = b.nombreBanco
                }).ToList();
            
            return new SelectList(bancos, "Value", "Text", selectedValue);
        }

        private SelectList ObtenerPeriocidadesPagoSelectList(object selectedValue = null)
        {
            var periocidades = new List<SelectListItem>
            {
                new SelectListItem { Value = "Quincenal", Text = "Quincenal" },
                new SelectListItem { Value = "Mensual", Text = "Mensual" },
                new SelectListItem { Value = "Semanal", Text = "Semanal" }
            };
            
            return new SelectList(periocidades, "Value", "Text", selectedValue);
        }

        #endregion
    }
}