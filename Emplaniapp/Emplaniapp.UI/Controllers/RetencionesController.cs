using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.Abstracciones.InterfacesParaUI.Retenciones;
using Emplaniapp.Abstracciones.InterfacesParaUI.TipoRetencion;

using Emplaniapp.LogicaDeNegocio.Retenciones;
using Emplaniapp.LogicaDeNegocio.Tipo_Retencion;
using Emplaniapp.LogicaDeNegocio.Empleado.ObtenerEmpleadoPorId;

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
        private ApplicationUserManager _userManager;

        public RetencionesController()
        {
            // Ahora usamos los ctors parameterless
            _datosPersonalesLN = new ObtenerEmpleadoPorIdLN();
            _listarTipoRetencionLN = new ListarTipoRetencionLN();
            _obtenerIdTipoRetencionLN = new ObtenerIdTipoRetencionLN();

            _listarRetencionesLN = new ListarRetencionesLN();
            _agregarRetencionLN = new AgregarRetencionLN();
            _editarRetencionLN = new EditarRetencionLN();
            _obtenerRetencionLN = new ObtenerRetencionPorIdLN();
            _eliminarRetencionLN = new EliminarRetencionLN();
        }

        public RetencionesController(ApplicationUserManager userManager)
            : this()
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager
                   ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        // 1) Detalles / Listado
        public ActionResult Detalles(int? id, string seccion = "Retenciones")
        {
            int idEmpleado;
            if (id.HasValue)
                idEmpleado = id.Value;
            else
            {
                var claims = User.Identity as ClaimsIdentity;
                var claim = claims?.FindFirst("idEmpleado");
                if (claim == null || !int.TryParse(claim.Value, out idEmpleado))
                    return new HttpStatusCodeResult(400, "No se pudo identificar al empleado.");
            }

            var emp = _datosPersonalesLN.ObtenerEmpleadoPorId(idEmpleado);
            if (emp == null) return HttpNotFound();

            var ret = _listarRetencionesLN.Listar(idEmpleado);
            ViewBag.Seccion = seccion;
            return View(Tuple.Create(emp, ret));
        }

        // 2) Create
        public ActionResult Create(int idEmpleado)
        {
            var emp = _datosPersonalesLN.ObtenerEmpleadoPorId(idEmpleado);
            if (emp == null) return HttpNotFound();

            var tipos = _listarTipoRetencionLN.Listar();
            var vm = new AgregarRetencionViewModel
            {
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

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(AgregarRetencionViewModel vm)
        {
            if (!ModelState.IsValid)
                return Create(vm.IdEmpleado);

            vm.MontoRetencion = vm.SalarioBase * ((decimal)vm.Porcentaje / 100m);

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

        // 3) Editar
        public ActionResult Edit(int id)
        {
            var ent = _obtenerRetencionLN.Obtener(id);
            if (ent == null) return HttpNotFound();

            var emp = _datosPersonalesLN.ObtenerEmpleadoPorId(ent.idEmpleado);
            var tipos = _listarTipoRetencionLN.Listar();
            var vm = new EditarRetencionViewModel
            {
                IdRetencion = ent.idRetencion,
                IdEmpleado = ent.idEmpleado,
                NombreEmpleado = $"{emp.nombre} {emp.primerApellido}",
                IdTipoRetencion = ent.idTipoRetencio,
                Porcentaje = (decimal)tipos.First(t => t.Id == ent.idTipoRetencio).porcentajeRetencion,
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

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(EditarRetencionViewModel vm)
        {
            if (!ModelState.IsValid)
                return Edit(vm.IdRetencion);

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

        // 4) Eliminar
        public ActionResult Delete(int id)
        {
            var ent = _obtenerRetencionLN.Obtener(id);
            return ent == null ? (ActionResult)HttpNotFound() : View(ent);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var ent = _obtenerRetencionLN.Obtener(id);
            _eliminarRetencionLN.EliminarRetencion(ent.idRetencion);
            return RedirectToAction("Detalles", new { id = ent.idEmpleado });
        }
    }
}
