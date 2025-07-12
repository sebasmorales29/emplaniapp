// Controllers/RetencionesController.cs
using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.Abstracciones.InterfacesParaUI.Retenciones;
using Emplaniapp.Abstracciones.InterfacesParaUI.TipoRetencion;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.LogicaDeNegocio.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.LogicaDeNegocio.Retenciones;
using Emplaniapp.LogicaDeNegocio.Tipo_Retencion;
using Emplaniapp.UI.Models;

namespace Emplaniapp.UI.Controllers
{
    [Authorize]
    public class RetencionesController : Controller
    {
        private readonly IObtenerEmpleadoPorIdLN _datosPersonalesLN;
        private readonly IListarTipoRetencionLN _listarTipoRetencionLN;
        private readonly IObtenerIdTipoRetencionLN _obtenerIdTipoRetencionLN;
        private readonly IListarRetencionesLN _listarRetencionesLN;
        private readonly IAgregarRetencionLN _agregarRetencionLN;
        private readonly IEditarRetencionLN _editarRetencionLN;
        private readonly IObtenerRetencionPorIdLN _obtenerRetencionLN;
        private readonly IEliminarRetencionLN _eliminarRetencionLN;

        public RetencionesController()
        {
            _datosPersonalesLN = new ObtenerEmpleadoPorIdLN();
            _listarTipoRetencionLN = new ListarTipoRetencionLN();
            _obtenerIdTipoRetencionLN = new ObtenerIdTipoRetencionLN();
            _listarRetencionesLN = new ListarRetencionesLN();
            _agregarRetencionLN = new AgregarRetencionLN();
            _editarRetencionLN = new EditarRetencionLN();
            _obtenerRetencionLN = new ObtenerRetencionPorIdLN();
            _eliminarRetencionLN = new EliminarRetencionLN();
        }

        // Listado/Detalles
        public ActionResult Detalles(int? id, string seccion = "Retenciones")
        {
            int idEmpleado = id ?? int.Parse(
                (User.Identity as ClaimsIdentity)?.FindFirst("idEmpleado")?.Value ?? "0"
            );

            var emp = _datosPersonalesLN.ObtenerEmpleadoPorId(idEmpleado);
            if (emp == null) return HttpNotFound();

            var ret = _listarRetencionesLN.Listar(idEmpleado);
            ViewBag.Seccion = seccion;
            return View(Tuple.Create(emp, ret));
        }

        // GET: Create
        public ActionResult Create(int idEmpleado)
        {
            var emp = _datosPersonalesLN.ObtenerEmpleadoPorId(idEmpleado);
            if (emp == null) return HttpNotFound();

            var tipos = _listarTipoRetencionLN.Listar();
            var vm = new RetencionViewModel
            {
                IdRetencion = 0,
                IdEmpleado = idEmpleado,
                NombreEmpleado = $"{emp.nombre} {emp.primerApellido}",
                SalarioBase = emp.salarioAprobado,
                FechaRetencion = DateTime.Today,
                TiposRetencion = tipos.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = $"{t.nombreTipoRetencion} ({t.porcentajeRetencion}%)"
                })
            };
            return View(vm);
        }

        // POST: Create
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(RetencionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.TiposRetencion = _listarTipoRetencionLN.Listar()
                    .Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = $"{t.nombreTipoRetencion} ({t.porcentajeRetencion}%)"
                    });
                return View(vm);
            }

            // Recalcular porcentaje y monto
            var tipoDto = _obtenerIdTipoRetencionLN.Obtener(vm.IdTipoRetencion);
            vm.Porcentaje = tipoDto != null
                                ? (decimal)tipoDto.porcentajeRetencion
                                : 0m;
            vm.MontoRetencion = vm.SalarioBase * (vm.Porcentaje / 100m);

            var dto = new RetencionCrearDto
            {
                idEmpleado = vm.IdEmpleado,
                idTipoRetencio = vm.IdTipoRetencion,
                rebajo = vm.MontoRetencion,
                fechaRetencio = vm.FechaRetencion
            };
            _agregarRetencionLN.AgregarRetencion(dto);

            return RedirectToAction("Detalles", new { id = vm.IdEmpleado });
        }

        // GET: Edit
        public ActionResult Edit(int id)
        {
            var ent = _obtenerRetencionLN.Obtener(id);
            var emp = _datosPersonalesLN.ObtenerEmpleadoPorId(ent.idEmpleado);
            var tipos = _listarTipoRetencionLN.Listar();

            var pct = (decimal)tipos.First(t => t.Id == ent.idTipoRetencio)
                                     .porcentajeRetencion;

            var vm = new RetencionViewModel
            {
                IdRetencion = ent.idRetencion,
                IdEmpleado = ent.idEmpleado,
                NombreEmpleado = $"{emp.nombre} {emp.primerApellido}",
                SalarioBase = emp.salarioAprobado,
                IdTipoRetencion = ent.idTipoRetencio,
                Porcentaje = pct,
                MontoRetencion = ent.rebajo,
                FechaRetencion = ent.fechaRetencio,
                TiposRetencion = tipos.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = $"{t.nombreTipoRetencion} ({t.porcentajeRetencion}%)"
                })
            };
            return View(vm);
        }

        // POST: Edit
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(RetencionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.TiposRetencion = _listarTipoRetencionLN.Listar()
                    .Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = $"{t.nombreTipoRetencion} ({t.porcentajeRetencion}%)"
                    });
                return View(vm);
            }

            var tipoDto = _obtenerIdTipoRetencionLN.Obtener(vm.IdTipoRetencion);
            vm.Porcentaje = tipoDto != null
                                ? (decimal)tipoDto.porcentajeRetencion
                                : 0m;
            vm.MontoRetencion = vm.SalarioBase * (vm.Porcentaje / 100m);

            var dto = new RetencionEditarDto
            {
                idRetencion = vm.IdRetencion,
                idTipoRetencio = vm.IdTipoRetencion,
                rebajo = vm.MontoRetencion,
                fechaRetencio = vm.FechaRetencion
            };
            _editarRetencionLN.EditarRetencion(dto);

            return RedirectToAction("Detalles", new { id = vm.IdEmpleado });
        }

        // GET: Delete
        public ActionResult Delete(int id)
        {
            var ent = _obtenerRetencionLN.Obtener(id);
            if (ent == null) return HttpNotFound();
            return View(ent);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var ent = _obtenerRetencionLN.Obtener(id);
            _eliminarRetencionLN.EliminarRetencion(ent.idRetencion);
            return RedirectToAction("Detalles", new { id = ent.idEmpleado });
        }
    }
}
