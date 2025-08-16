using System.Threading.Tasks;
using System.Web.Mvc;
using Emplaniapp.UI.Attributes;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.Abstracciones.InterfacesParaUI.TipoRetencion;
using Emplaniapp.LogicaDeNegocio.Tipo_Remuneracion;
using Emplaniapp.LogicaDeNegocio.Tipo_Retencion;
using Emplaniapp.Abstracciones.InterfacesParaUI.Tipo_Remuneracion;
using System.Collections.Generic;
using System;

namespace Emplaniapp.UI.Controllers
{
    [ActiveRoleAuthorize("Administrador", "Contador")]
    public class VariablesFinancierasController : Controller
    {
        readonly IListarTipoRemuneracionLN _listarRemuLN = new ListarTipoRemuneracionLN();
        readonly IAgregarTipoRemuneracionLN _agregarRemuLN = new AgregarTipoRemuneracionLN();
        readonly IEditarTipoRemuneracionLN _editarRemuLN = new EditarTipoRemuneracionLN();
        readonly IObtenerIdTipoRemuneracionLN _obtenerRemuLN = new ObtenerIdTipoRemuneracionLN();
        readonly IEliminarTipoRemuneracionLN _eliminarRemuLN = new EliminarTipoRemuneracionLN();

        readonly IListarTipoRetencionLN _listarRetenLN = new ListarTipoRetencionLN();
        readonly IAgregarTipoRetencionLN _agregarRetenLN = new AgregarTipoRetencionLN();
        readonly IEditarTipoRetencionLN _editarRetenLN = new EditarTipoRetencionLN();
        readonly IObtenerIdTipoRetencionLN _obtenerRetenIdLN = new ObtenerIdTipoRetencionLN();
        readonly IEliminarTipoRetencionLN _eliminarRetenLN = new EliminarTipoRetencionLN();

        // GET: VariablesFinancieras
        public ActionResult Index()
        {
            var vars = new Tuple<
                List<TipoRemuneracionDto>,
                List<TipoRetencionDto>
            >(
                _listarRemuLN.Listar(),
                _listarRetenLN.Listar()
            );
            return View(vars);
        }

        // Remuneraciones CRUD...
        public ActionResult CreateRemu() => View();
        [HttpPost]
        public async Task<ActionResult> CreateRemu(TipoRemuneracionDto dto)
        {
            await _agregarRemuLN.Guardar(dto);
            return RedirectToAction("Index");
        }
        public ActionResult EditRemu(int id) => View(_obtenerRemuLN.Obtener(id));
        [HttpPost]
        public ActionResult EditRemu(TipoRemuneracionDto dto)
        {
            _editarRemuLN.Editar(dto);
            return RedirectToAction("Index");
        }
        public ActionResult DeleteRemu(int id) => View(_obtenerRemuLN.Obtener(id));
        [HttpPost]
        public ActionResult DeleteRemu(TipoRemuneracionDto dto)
        {
            _eliminarRemuLN.Eliminar(dto.Id);
            return RedirectToAction("Index");
        }

        // Retenciones CRUD...
        public ActionResult CreateReten() => View();
        [HttpPost]
        public async Task<ActionResult> CreateReten(TipoRetencionDto dto)
        {
            await _agregarRetenLN.Guardar(dto);
            return RedirectToAction("Index");
        }
        public ActionResult EditReten(int id) => View(_obtenerRetenIdLN.Obtener(id));
        [HttpPost]
        public ActionResult EditReten(TipoRetencionDto dto)
        {
            _editarRetenLN.Editar(dto);
            return RedirectToAction("Index");
        }
        public ActionResult DeleteReten(int id) => View(_obtenerRetenIdLN.Obtener(id));
        [HttpPost]
        public ActionResult DeleteReten(TipoRetencionDto dto)
        {
            _eliminarRetenLN.Eliminar(dto.Id);
            return RedirectToAction("Index");
        }
    }
}
