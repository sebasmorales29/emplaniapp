using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Emplaniapp.Abstracciones.InterfacesParaUI;
using Emplaniapp.Abstracciones.InterfacesParaUI.Cargos.ListarCargos;
using Emplaniapp.Abstracciones.InterfacesParaUI.Estados.ListarEstados;
using Emplaniapp.Abstracciones.InterfacesParaUI.General.FiltrarEmpleados;
using Emplaniapp.Abstracciones.InterfacesParaUI.General.ObtenerTotalEmpleados;
using Emplaniapp.Abstracciones.InterfacesParaUI.Hoja_Resumen.ListarHojaResumen;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.LogicaDeNegocio;
using Emplaniapp.LogicaDeNegocio.Cargos.ListarCargos;
using Emplaniapp.LogicaDeNegocio.Estados.ListarEstados;
using Emplaniapp.LogicaDeNegocio.General.FiltrarEmpleados;
using Emplaniapp.LogicaDeNegocio.General.ObtenerTotalEmpleados;
using Emplaniapp.LogicaDeNegocio.Hoja_Resumen.ListarHojaResumen;

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

        public HojaResumenController()
        {
            _listarHojaResumenLN = new listarHojaResumenLN();
            _datosPersonalesLN = new DatosPersonalesLN();
            _listarCargosLN = new listarCargosLN();
            _listarEstadosLN = new listarEstadosLN();
            _filtrarEmpleadosLN = new filtrarEmpleadosLN();
            _obtenerTotalEmpleadosLN = new obtenerTotalEmpleadosLN();
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
    }
}

