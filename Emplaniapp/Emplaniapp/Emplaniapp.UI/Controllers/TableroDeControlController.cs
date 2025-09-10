using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Emplaniapp.Abstracciones.InterfacesParaUI.General.ObtenerTotalEmpleados;
using Emplaniapp.Abstracciones.InterfacesParaUI.Hoja_Resumen.ListarHojaResumen;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.LogicaDeNegocio.General.ObtenerTotalEmpleados;
using Emplaniapp.LogicaDeNegocio.Hoja_Resumen.ListarHojaResumen;

namespace Emplaniapp.UI.Controllers
{
    [Authorize]
    public class TableroDeControlController : Controller
    {
        private readonly IlistarHojaResumenLN _listarHojaResumenLN;
        private readonly IObtenerTotalEmpleadosLN _obtenerTotalEmpleadosLN;

        public TableroDeControlController()
        {
            _listarHojaResumenLN = new listarHojaResumenLN();
            _obtenerTotalEmpleadosLN = new obtenerTotalEmpleadosLN();
        }

        // Vista
        public ActionResult Index()
        {
            return View();
        }

        // ---------- API JSON para gráficos ----------

        [HttpGet]
        public JsonResult Kpis()
        {
            var resumen = _listarHojaResumenLN.ObtenerHojasResumen() ?? new List<HojaResumenDto>();

            var kpis = new
            {
                totalEmpleados = _obtenerTotalEmpleadosLN.ObtenerTotalEmpleados(null, null, null, true),
                nominaActual = resumen.Sum(r => r.SalarioNeto),
                totalRemuneraciones = resumen.Sum(r => r.TotalRemuneraciones),
                totalRetenciones = resumen.Sum(r => r.TotalRetenciones)
            };

            return Json(kpis, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Estados()
        {
            var resumen = _listarHojaResumenLN.ObtenerHojasResumen() ?? new List<HojaResumenDto>();

            var grupos = resumen
                .GroupBy(r => (r.nombreEstado ?? "Sin estado").Trim())
                .Select(g => new { label = g.Key, value = g.Count() })
                .OrderByDescending(x => x.value)
                .ToList();

            return Json(new
            {
                labels = grupos.Select(x => x.label).ToArray(),
                data = grupos.Select(x => x.value).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult TopSalarios(int top = 5)
        {
            var resumen = _listarHojaResumenLN.ObtenerHojasResumen() ?? new List<HojaResumenDto>();

            var topList = resumen
                .OrderByDescending(r => r.SalarioNeto)
                .Take(top)
                .Select(r => new { nombre = r.NombreEmpleado, valor = r.SalarioNeto })
                .ToList();

            return Json(new
            {
                labels = topList.Select(x => x.nombre).ToArray(),
                data = topList.Select(x => x.valor).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
