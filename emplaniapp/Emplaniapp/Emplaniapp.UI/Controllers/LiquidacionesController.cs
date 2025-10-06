using Emplaniapp.Abstracciones.InterfacesParaUI;
using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.Abstracciones.InterfacesParaUI.Liquidaciones;
using Emplaniapp.Abstracciones.ModelosAD;
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
        private readonly IMostrarCalculosLiqLN _calculosPrevios;
        private readonly IGuardarLiquidacionLN _guardarLiquidacion;
        private readonly IEditarLiquidacionLN _editarLiq;

        public LiquidacionesController()
        {
            _obtenerEmpleado = new ObtenerEmpleadoPorIdLN();
            _obtenerLiqPorEmpleado = new ObtenerLiqPorEmpleadoIdLN();
            _calculosPrevios = new MostrarCalculosLiqLN();
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
                         
            // Se verifica si existe ya la liquidación
            var liqExiste = _obtenerLiqPorEmpleado.ObtenerPorEmpleadoID(emp.idEmpleado);
            if (liqExiste != null)
            {
                return View(Tuple.Create(emp, liqExiste));
            }
            else
            {
                // Se crea desde cero, pero no se guarda en BD
                var liq = _calculosPrevios.MostrarLiquidacionParcial(emp);
                return View(Tuple.Create(emp, liq));
            }
        }


        // Agregar ------------------------------------------------------------------------------------------------------------
        public ActionResult AgregarDatos(int id) //  Se crea la vista
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
            LiquidacionDto liq = _calculosPrevios.MostrarLiquidacionParcial(emp);  // liquidación previa fuera de bd
            return View(liq);
        }
        [HttpPost]
        public async Task<ActionResult> AgregarDatos(LiquidacionDto liq) // Se agregan los datos
        {
            LiquidacionDto liquidacion = _calculosPrevios.MostrarLiquidacionTotal(1,liq); // Se genera primer cálculo
            await _guardarLiquidacion.Guardar(liquidacion);                               // y se guarda en base de datos
            return RedirectToAction("Detalles", "Liquidaciones",                          // Se redirige a la página principal
                new { id = liq.idEmpleado, seccion = "Liquidacion" });
        }


        // Editar -------------------------------------------------------------------------------------------------------------
        public ActionResult EditDatos(int id) // Vista con 
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
            LiquidacionDto liq = _obtenerLiqPorEmpleado.ObtenerPorEmpleadoID(id);           // liquidación guardada en bd
            return View(liq);
        }
        [HttpPost]
        public ActionResult EditDatos(LiquidacionDto liq)                                   // Se edita en base de datos
        {
            LiquidacionDto liquid = _calculosPrevios.MostrarLiquidacionTotal(2, liq);       // Obtengo la liquidación alterada
            _editarLiq.Editar(liquid);                                                      // Guardo el cambio en BD
            return RedirectToAction("Detalles", "Liquidaciones",
                new { id = liquid.idEmpleado, seccion = "Liquidacion" });
        }



        public ActionResult GuardarDatos(int id)
        {
            LiquidacionDto liq = _obtenerLiqPorEmpleado.ObtenerPorEmpleadoID(id);           // liquidación guardada en bd
            return View(liq);
        }
        [HttpPost]
        public ActionResult GuardarDatos(LiquidacionDto liq)
        {
            // Se modifica el estado de la liquidación a 1 (activo)
            _editarLiq.EditarFinal(liq);

            // Se modifica la fecha de salida
            EmpleadoDto emp = _obtenerEmpleado.ObtenerEmpleadoPorId(liq.idEmpleado);

            return RedirectToAction("Detalles", "Liquidaciones",
                new { id = liq.idEmpleado, seccion = "Liquidacion" });
        }



    }
}
