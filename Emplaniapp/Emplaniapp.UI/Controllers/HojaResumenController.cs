﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Host.SystemWeb;
using Emplaniapp.Abstracciones.Entidades;
using Emplaniapp.Abstracciones.InterfacesParaUI;
using Emplaniapp.Abstracciones.InterfacesParaUI.Cargos.ListarCargos;
using Emplaniapp.Abstracciones.InterfacesParaUI.Estados.ListarEstados;
using Emplaniapp.Abstracciones.InterfacesParaUI.General.FiltrarEmpleados;
using Emplaniapp.Abstracciones.InterfacesParaUI.General.ObtenerTotalEmpleados;
using Emplaniapp.Abstracciones.InterfacesParaUI.Hoja_Resumen.ListarHojaResumen;
using Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones.CrearRemuneraciones;
using Emplaniapp.Abstracciones.InterfacesParaUI.Tipo_Remuneracion;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.LogicaDeNegocio;
using Emplaniapp.LogicaDeNegocio.Cargos.ListarCargos;
using Emplaniapp.LogicaDeNegocio.Estados.ListarEstados;
using Emplaniapp.LogicaDeNegocio.General.FiltrarEmpleados;
using Emplaniapp.LogicaDeNegocio.General.ObtenerTotalEmpleados;
using Emplaniapp.LogicaDeNegocio.Hoja_Resumen.ListarHojaResumen;
using Emplaniapp.LogicaDeNegocio.Remuneraciones.CrearRemuneraciones;
using Emplaniapp.LogicaDeNegocio.Tipo_Remuneracion;

namespace Emplaniapp.UI.Controllers
{
    [Authorize(Roles = "Administrador, Contador")]
    public class HojaResumenController : Controller
    {
        private IlistarHojaResumenLN _listarHojaResumenLN;
        private IDatosPersonalesLN _datosPersonalesLN;
        private IListarCargosLN _listarCargosLN;
        private IListarEstadosLN _listarEstadosLN;
        private IFiltrarEmpleadosLN _filtrarEmpleadosLN;
        private IObtenerTotalEmpleadosLN _obtenerTotalEmpleadosLN;
        private ICrearRemuneracionesLN _crearRemuneracionesLN;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public HojaResumenController()
        {
            _listarHojaResumenLN = new listarHojaResumenLN();
            _datosPersonalesLN = new DatosPersonalesLN();
            _listarCargosLN = new listarCargosLN();
            _listarEstadosLN = new listarEstadosLN();
            _filtrarEmpleadosLN = new filtrarEmpleadosLN();
            _obtenerTotalEmpleadosLN = new obtenerTotalEmpleadosLN();
            _crearRemuneracionesLN = new CrearRemuneracionesLN();
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
            return _listarCargosLN.ObtenerCargos()
                .Select(p => new SelectListItem
                {
                    Value = p.idCargo.ToString(),
                    Text = p.nombreCargo
                }).ToList();
        }

        // GET: Filtrar
        public ActionResult listarHojaResumen()
        {
            List<HojaResumenDto> laListaDeHojaDeResumen = _listarHojaResumenLN.ObtenerHojasResumen();
            ViewBag.Cargos = ObtenerCargos();
            ViewBag.TotalEmpleados = _obtenerTotalEmpleadosLN.ObtenerTotalEmpleados(null, null,null,true);
            return View(laListaDeHojaDeResumen);
        }

        [HttpPost]
        public ActionResult Filtrar(string filtro, int? idCargo)
        {
            var listaFiltrada = _filtrarEmpleadosLN.ObtenerFiltrado<HojaResumenDto>(filtro, idCargo, null);
            ViewBag.Filtro = filtro;
            ViewBag.idCargo = idCargo;
            ViewBag.Cargos = ObtenerCargos();
            ViewBag.TotalEmpleados = _obtenerTotalEmpleadosLN.ObtenerTotalEmpleados(filtro, idCargo, null, true);
            return View("listarHojaResumen", listaFiltrada);
        }

        // GET: HojaResumen/VerDetalles/5
        public ActionResult VerDetalles(int id)
        {
            // Redirige al controlador de DatosPersonales para ver los detalles del empleado
            return RedirectToAction("Detalles", "DatosPersonales", new { id = id });
        }

        // GET: HojaResumen/GenerarRemurenacionesATodosLosEmpleados
        [HttpPost]
        public ActionResult Generar()
        {
            DateTime fechaProceso = DateTime.Today;
            //DateTime fechaProceso = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 10);

            int dia = fechaProceso.Day;

            if (dia <= 15)
            {
                fechaProceso = new DateTime(fechaProceso.Year, fechaProceso.Month, 15);
            }
            else
            {
                fechaProceso = new DateTime(fechaProceso.Year, fechaProceso.Month,
                                          DateTime.DaysInMonth(fechaProceso.Year, fechaProceso.Month));
            }

            try
            {
                var remuneraciones = _crearRemuneracionesLN.GenerarRemuneracionesQuincenales(fechaProceso);

                TempData["SuccessMessage"] = $"Remuneraciones generadas exitosamente para la fecha {fechaProceso:dd/MM/yyyy}.";
                return RedirectToAction("listarHojaResumen");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al generar remuneraciones: " + ex.Message;
                return RedirectToAction("listarHojaResumen");
            }
        }

        // POST: Validar contraseña de administrador
        [HttpPost]
        [Authorize(Roles = "Administrador")]
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

    }
}

