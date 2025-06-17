using Emplaniapp.Abstracciones.InterfacesParaUI.Tipo_Remuneracion;
using Emplaniapp.Abstracciones.InterfacesParaUI.Tipo_Retenciones;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.LogicaDeNegocio.Tipo_Remuneracion;
using Emplaniapp.LogicaDeNegocio.Tipo_Retencion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Emplaniapp.UI.Controllers
{
    [Authorize(Roles = "Administrador, Contador")]
    public class VariablesFinancierasController : Controller
    {

        // Interfaces Remuneraciones ---------------------------------
        IListarTipoRemuneracionLN _listarTRemuLN;
        IAgregarTipoRemuneracionLN agregarTRemuLN;
        IEditarTipoRemuneracionLN editarTRemuLN;
        IObtenerIdTipoRemuneracionLN obtenerIdTRemuLN;
        IEliminarTipoRemuneracionLN eliminarTRemuLN;

        // Interfaces Retenciones ---------------------------------
        IListarTipoRetencionLN _listarTRetenLN;
        IAgregarTipoRetencionLN agregarTRetenLN;
        IEditarTipoRetencionLN editarTRetenLN;
        IObtenerIdTipoRetencionLN obtenerIdTRetenLN;
        IEliminarTipoRetencionLN eliminarTRetenLN;


        public VariablesFinancierasController()
        {
            _listarTRemuLN = new ListarTipoRemuneracionLN();
            _listarTRetenLN = new ListarTipoRetencionLN();

            agregarTRemuLN = new AgregarTipoRemuneracionLN();
            editarTRemuLN = new EditarTipoRemuneracionLN();
            obtenerIdTRemuLN = new ObtenerIdTipoRemuneracionLN();
            eliminarTRemuLN = new EliminarTipoRemuneracionLN();

            agregarTRetenLN = new AgregarTipoRetencionLN();
            editarTRetenLN = new EditarTipoRetencionLN();
            obtenerIdTRetenLN = new ObtenerIdTipoRetencionLN();
            eliminarTRetenLN = new EliminarTipoRetencionLN();

        }



        // GET: VariablesFinancieras
        public ActionResult Index()
        {
            var variables = new Tuple
                < List<TipoRemuneracionDto>, List<TipoRetencionDto> >
                (_listarTRemuLN.Listar(), _listarTRetenLN.Listar());
            return View(variables);
        }



        // REMUNERACIONES  ---------------------------------------------


        // GET: VariablesFinancieras/Create
        public ActionResult CreateRemu()
        {
            return View();
        }

        // POST: VariablesFinancieras/Create
        [HttpPost]
        public async Task<ActionResult> CreateRemu(TipoRemuneracionDto tipoRemu)
        {
            try
            {
                // TODO: Add insert logic here
                await agregarTRemuLN.Guardar(tipoRemu);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: VariablesFinancieras/Edit/5
        public ActionResult EditRemu(int id)
        {
            TipoRemuneracionDto tipoRemu = obtenerIdTRemuLN.Obtener(id);
            return View(tipoRemu);
        }

        // POST: VariablesFinancieras/Edit/5
        [HttpPost]
        public ActionResult EditRemu(TipoRemuneracionDto tipoRemu)
        {
            try
            {
                // TODO: Add update logic here
                editarTRemuLN.Editar(tipoRemu);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: VariablesFinancieras/Delete/5
        public ActionResult DeleteRemu(int id)
        {
            TipoRemuneracionDto tipoRemu = obtenerIdTRemuLN.Obtener(id);
            return View(tipoRemu);
        }

        // POST: VariablesFinancieras/Delete/5
        [HttpPost]
        public ActionResult DeleteRemu(TipoRemuneracionDto tipoRemu)
        {
            try
            {
                eliminarTRemuLN.Eliminar(tipoRemu.Id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }




        // RETENCIONES  ---------------------------------------------


        // GET: VariablesFinancieras/Create
        public ActionResult CreateReten()
        {
            return View();
        }

        // POST: VariablesFinancieras/Create
        [HttpPost]
        public async Task<ActionResult> CreateReten(TipoRetencionDto tipoRet)
        {
            try
            {
                // TODO: Add insert logic here
                await agregarTRetenLN.Guardar(tipoRet);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: VariablesFinancieras/Edit/5
        public ActionResult EditReten(int id)
        {
            TipoRetencionDto tipoReten = obtenerIdTRetenLN.Obtener(id);
            return View(tipoReten);
        }

        // POST: VariablesFinancieras/Edit/5
        [HttpPost]
        public ActionResult EditReten(TipoRetencionDto tipoReten)
        {
            try
            {
                // TODO: Add update logic here
                editarTRetenLN.Editar(tipoReten);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: VariablesFinancieras/Delete/5
        public ActionResult DeleteReten(int id)
        {
            TipoRetencionDto tipoReten = obtenerIdTRetenLN.Obtener(id);
            return View(tipoReten);
        }

        // POST: VariablesFinancieras/Delete/5
        [HttpPost]
        public ActionResult DeleteReten(TipoRetencionDto tipoReten)
        {
            try
            {
                eliminarTRetenLN.Eliminar(tipoReten.Id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }



    }
}
