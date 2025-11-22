using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly IObtenerRetencionPorIdLN _obtenerRetencionLN;
        private readonly IEditarRetencionLN _editarRetencionLN;
        private readonly IEliminarRetencionLN _eliminarRetencionLN;

        public RetencionesController()
        {
            _datosPersonalesLN = new ObtenerEmpleadoPorIdLN();
            _listarTipoRetencionLN = new ListarTipoRetencionLN();
            _obtenerIdTipoRetencionLN = new ObtenerIdTipoRetencionLN();
            _listarRetencionesLN = new ListarRetencionesLN();
            _agregarRetencionLN = new AgregarRetencionLN();
            _obtenerRetencionLN = new ObtenerRetencionPorIdLN();
            _editarRetencionLN = new EditarRetencionLN();
            _eliminarRetencionLN = new EliminarRetencionLN();
        }

        // --- 1) Detalles: página principal ---
        public ActionResult Detalles(int? id)
        {
            var idEmpleado = id
                ?? int.Parse((User.Identity as ClaimsIdentity)?
                                 .FindFirst("idEmpleado")?.Value
                             ?? "0");

            var emp = _datosPersonalesLN.ObtenerEmpleadoPorId(idEmpleado);
            if (emp == null) return HttpNotFound();

            var lista = _listarRetencionesLN.Listar(idEmpleado).ToList();
            return View(Tuple.Create(emp, lista));
        }

        // --- 2) Lista parcial ---
        public ActionResult Lista(int idEmpleado)
        {
            var lista = idEmpleado > 0
                ? _listarRetencionesLN.Listar(idEmpleado).ToList()
                : new List<RetencionDto>();
            return PartialView("_ListaRetenciones", lista);
        }

        // --- 3) Create GET ---
        public ActionResult Create(int? idEmpleado)
        {
            if (!idEmpleado.HasValue || idEmpleado <= 0)
                return new HttpStatusCodeResult(400, "Falta idEmpleado");

            var emp = _datosPersonalesLN.ObtenerEmpleadoPorId(idEmpleado.Value);
            if (emp == null) return HttpNotFound();

            var tipos = _listarTipoRetencionLN.Listar();
            var vm = new RetencionViewModel
            {
                IdRetencion = 0,
                IdEmpleado = idEmpleado.Value,
                NombreEmpleado = $"{emp.nombre} {emp.primerApellido}",
                SalarioBase = emp.salarioAprobado,
                FechaRetencion = DateTime.Today,
                TiposRetencion = tipos.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = $"{t.nombreTipoRetencion} ({t.porcentajeRetencion}%)"
                })
            };
            return PartialView("_CrearRetencionModal", vm);
        }

        // --- 4) Create POST (AJAX JSON) ---
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(RetencionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { success = false, errors = errores });
            }

            var pct = Convert.ToDecimal(
                _obtenerIdTipoRetencionLN.Obtener(vm.IdTipoRetencion)
                    .porcentajeRetencion
            );
            vm.Porcentaje = pct;
            vm.MontoRetencion = vm.SalarioBase * pct / 100m;

            var dto = new RetencionCrearDto
            {
                idEmpleado = vm.IdEmpleado,
                idTipoRetencio = vm.IdTipoRetencion,
                rebajo = vm.MontoRetencion,
                fechaRetencio = vm.FechaRetencion
            };
            _agregarRetencionLN.AgregarRetencion(dto);

            return Json(new { success = true,
                redirectUrl = Url.Action("Retenciones", "Detalles", new { id = vm.IdEmpleado }),
                reload = true
            });
        }

        // --- 5) Edit GET ---
        public ActionResult Edit(int id)
        {
            var ent = _obtenerRetencionLN.Obtener(id);
            if (ent == null) return HttpNotFound();

            var emp = _datosPersonalesLN.ObtenerEmpleadoPorId(ent.idEmpleado);
            var tipos = _listarTipoRetencionLN.Listar();
            var pct = Convert.ToDecimal(
                tipos.First(t => t.Id == ent.idTipoRetencio)
                     .porcentajeRetencion
            );

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
            return PartialView("_EditarRetencionModal", vm);
        }

        // --- 6) Edit POST (AJAX JSON) ---
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(RetencionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { success = false, errors = errores });
            }

            var pct = Convert.ToDecimal(
                _obtenerIdTipoRetencionLN.Obtener(vm.IdTipoRetencion)
                    .porcentajeRetencion
            );
            vm.Porcentaje = pct;
            vm.MontoRetencion = vm.SalarioBase * pct / 100m;

            var dto = new RetencionEditarDto
            {
                idRetencion = vm.IdRetencion,
                idTipoRetencio = vm.IdTipoRetencion,
                rebajo = vm.MontoRetencion,
                fechaRetencio = vm.FechaRetencion
            };
            _editarRetencionLN.EditarRetencion(dto);

            return Json(new { success = true,
                redirectUrl = Url.Action("Retenciones", "Detalles", new { id = vm.IdEmpleado}),
                reload = true
            });
        }

        // --- 7) Delete GET ---
        public ActionResult Delete(int id)
        {
            var ent = _obtenerRetencionLN.Obtener(id);
            if (ent == null) return HttpNotFound();

            var emp = _datosPersonalesLN.ObtenerEmpleadoPorId(ent.idEmpleado);
            var vm = new RetencionViewModel
            {
                IdRetencion = ent.idRetencion,
                IdEmpleado = ent.idEmpleado,
                NombreEmpleado = $"{emp.nombre} {emp.primerApellido}",
                FechaRetencion = ent.fechaRetencio
            };
            return PartialView("_EliminarRetencionModal", vm);
        }

        // --- 8) Delete POST (AJAX JSON) ---
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(RetencionViewModel vm)
        {
            _eliminarRetencionLN.EliminarRetencion(vm.IdRetencion);
            return Json(new { success = true,
                redirectUrl = Url.Action("Retenciones", "Detalles", new { id = vm.IdEmpleado }),
                reload = true
            });
        }
    }
}
