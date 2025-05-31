using Emplaniapp.Abstracciones.InterfacesParaUI.Tipo_Remuneracion;
using Emplaniapp.Abstracciones.InterfacesParaUI.Tipo_Retenciones;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.LogicaDeNegocio.Tipo_Remuneracion;
using Emplaniapp.LogicaDeNegocio.Tipo_Retencion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Emplaniapp.UI.Controllers
{
    public class VariablesFinancierasController : Controller
    {

        // Interfaces ---------------------------------
        IListarTipoRemuneracionLN _listarTRemuLN;
        IListarTipoRetencionLN _listarTRetenLN;


        public VariablesFinancierasController()
        {
            _listarTRemuLN = new ListarTipoRemuneracionLN();
            _listarTRetenLN = new ListarTipoRetencionLN();
        }



        // GET: VariablesFinancieras
        public ActionResult Index()
        {
            var variables = new Tuple
                < List<TipoRemuneracionDto>, List<TipoRetencionDto> >
                (_listarTRemuLN.Listar(), _listarTRetenLN.Listar());
            return View(variables);
        }



        // GET: VariablesFinancieras/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VariablesFinancieras/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VariablesFinancieras/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: VariablesFinancieras/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VariablesFinancieras/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: VariablesFinancieras/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VariablesFinancieras/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
