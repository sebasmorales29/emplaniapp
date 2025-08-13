using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.Abstracciones.InterfacesParaUI.Liquidaciones;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.LogicaDeNegocio.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.LogicaDeNegocio.Liquidaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static System.Collections.Specialized.BitVector32;

namespace Emplaniapp.UI.Controllers
{
    [Authorize]
    public class LiquidacionesController : Controller
    {
        private readonly IObtenerEmpleadoPorIdLN _obtenerEmpleado;
        private readonly IObtenerLiqPorEmpleadoIdLN _obtenerLiqPorEmpleado;
        private readonly IMostrarCalculosPreviosLiqLN _calculosPrevios;
        private readonly IGuardarLiquidacionLN _guardarLiquidacion;
        private readonly IEditarLiquidacionLN _editarLiq;

        public LiquidacionesController()
        {
            _obtenerEmpleado = new ObtenerEmpleadoPorIdLN();
            _obtenerLiqPorEmpleado = new ObtenerLiqPorEmpleadoIdLN();
            _calculosPrevios = new MostrarCalculosPreviosLiqLN();
            _guardarLiquidacion = new GuardarLiquidacionLN();
            _editarLiq = new EditarLiquidacionLN();
        }


        public ActionResult Detalles(int? id, string seccion = "Liquidacion")
        {
            int idEmpleado = id ?? int.Parse(
                (User.Identity as ClaimsIdentity)?.FindFirst("idEmpleado")?.Value ?? "0"
            );

            var emp = _obtenerEmpleado.ObtenerEmpleadoPorId(idEmpleado);
            if (emp == null) return HttpNotFound();

            ViewBag.Seccion = seccion;
            ViewBag.MuestraLiq = true;
            // hacer un obtener liquidacion para que, si es nulo, entonces presente el mostrar:                

            var liqExiste = _obtenerLiqPorEmpleado.ObtenerPorEmpleadoID(emp.idEmpleado);
            if (liqExiste != null)
            {
                if (liqExiste.idEstado == 1) { ViewBag.MuestraLiq = false; }
                return View(Tuple.Create(emp, liqExiste));
            }
            else
            {
                var liq = _calculosPrevios.MostrarLiquidacionParcial(emp);


                return View(Tuple.Create(emp, liq));
            }
        }


        public ActionResult AgregarDatos(int id)
        {
            var motivos = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Despido Injustificado", Value = "Despido Injustificado" },
                    new SelectListItem { Text = "Despido Justificado", Value = "Despido Justificado" },
                    new SelectListItem { Text = "Renuncia", Value = "Renuncia" },
                    new SelectListItem { Text = "Renuncia por razones de empleador", Value = "Renuncia por razones de empleador" },
                    new SelectListItem { Text = "Pensión o muerte", Value = "Pensión o muerte" }
                };
            ViewBag.MotivosLiq = motivos;
            var emp = _obtenerEmpleado.ObtenerEmpleadoPorId(id);
            LiquidacionDto liq = _calculosPrevios.MostrarLiquidacionParcial(emp);
            return View(liq);
        }
        [HttpPost]
        public async Task<ActionResult> AgregarDatos(LiquidacionDto liq)
        {
            var emp = _obtenerEmpleado.ObtenerEmpleadoPorId(liq.idEmpleado);
            LiquidacionDto liquidacion = _calculosPrevios.MostrarLiquidacionTotal
                (emp, liq.fechaLiquidacion, liq.motivoLiquidacion);
            await _guardarLiquidacion.Guardar(liquidacion);
            return RedirectToAction("Detalles", "Liquidaciones",
                new { id = liq.idEmpleado, seccion = "Liquidacion" });
        }


        public ActionResult EditDatos(int id)
        {
            var motivos = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Despido Injustificado", Value = "Despido Injustificado" },
                    new SelectListItem { Text = "Despido Justificado", Value = "Despido Justificado" },
                    new SelectListItem { Text = "Renuncia", Value = "Renuncia" },
                    new SelectListItem { Text = "Renuncia por razones de empleador", Value = "Renuncia por razones de empleador" },
                    new SelectListItem { Text = "Pensión o muerte", Value = "Pensión o muerte" }
                };
            ViewBag.MotivosLiq = motivos;
            LiquidacionDto liq = _obtenerLiqPorEmpleado.ObtenerPorEmpleadoID(id);
            return View(liq);
        }
        [HttpPost]
        public ActionResult EditDatos(LiquidacionDto liq)
        {
            var emp = _obtenerEmpleado.ObtenerEmpleadoPorId(liq.idEmpleado);
            _editarLiq.Editar(emp, liq);
            return RedirectToAction("Detalles", "Liquidaciones",
                new { id = liq.idEmpleado, seccion = "Liquidacion" });
        }


        [HttpPost]
        public ActionResult GuardarDatos(LiquidacionDto liq)
        {
            var emp = _obtenerEmpleado.ObtenerEmpleadoPorId(liq.idEmpleado);
            _editarLiq.EditarFinal(emp, liq);
            return RedirectToAction("Detalles", "Liquidaciones",
                new { id = liq.idEmpleado, seccion = "Liquidacion" });
        }



    }
}
