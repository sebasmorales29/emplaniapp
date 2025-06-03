using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Emplaniapp.Abstracciones.InterfacesParaUI.Hoja_Resumen.ListarHojaResumen;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.LogicaDeNegocio.Hoja_Resumen.ListarHojaResumen;

namespace Emplaniapp.UI.Controllers
{
    public class HojaResumenController : Controller
    {
        private IlistarHojaResumenLN _listarHojaResumenLN;


        public HojaResumenController()
        {
            _listarHojaResumenLN = new listarHojaResumenLN();
        }

        private List<SelectListItem> ObtenerCargos()
        {
            return _listarHojaResumenLN.ObtenerCargos()
                .Select(p => new SelectListItem
                {
                    Value = p.idCargo.ToString(),
                    Text = p.nombreCargo
                }).ToList();
        }

        // GET: HojaResumen
        public ActionResult listarHojaResumen()
        {
            List<HojaResumenDto> laListaDeHojaDeResumen = _listarHojaResumenLN.ObtenerHojasResumen();
            ViewBag.Cargos = ObtenerCargos();
            return View(laListaDeHojaDeResumen);
        }

        [HttpPost]
        public ActionResult Filtrar(string filtro, int? idCargo)
        {
            var listaFiltrada = _listarHojaResumenLN.ObtenerFiltrado(filtro, idCargo);
            ViewBag.Filtro = filtro;
            ViewBag.idCargo = idCargo;
            ViewBag.Cargos = ObtenerCargos();
            return View("listarHojaResumen", listaFiltrada);
        }


        // GET: HojaResumen/VerDetalles/5
        public ActionResult VerDetalles(int id)
        {
            // Redirige al controlador de DatosPersonales para ver los detalles del empleado
            return RedirectToAction("Detalles", "DatosPersonales", new { id = id });
        }

        // GET: HojaResumen/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HojaResumen/Create
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

        // GET: HojaResumen/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HojaResumen/Edit/5
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

        // GET: HojaResumen/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HojaResumen/Delete/5
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

         /** #region Métodos Auxiliares

        // Método para obtener la lista de empleados (simulado)
        private List<EmpleadoDto> ObtenerEmpleados()
        {
            // Crear una lista de empleados (datos de ejemplo)
            return new List<EmpleadoDto>
            {
                new EmpleadoDto
                {
                    idEmpleado = 1,
                    nombre = "Yazmin",
                    segundoNombre = "",
                    primerApellido = "Rivera",
                    segundoApellido = "Rodríguez",
                    fechaNacimiento = new DateTime(1992, 11, 23),
                    cedula = 128048070,
                    numeroTelefonico = "7821-9903",
                    correoInstitucional = "yazriv@gmail.com",
                    idDireccion = 1,
                    idCargo = 2,
                    nombreCargo = "Contador",
                    fechaContratacion = new DateTime(2021, 9, 9),
                    fechaSalida = null,
                    periocidadPago = "Quincenal",
                    salarioDiario = 13400,
                    salarioAprobado = 1545000,
                    salarioPorMinuto = 27.91,
                    salarioPoHora = 1675.00,
                    salarioPorHoraExtra = 3350.00,
                    idMoneda = 1,
                    nombreMoneda = "Colones",
                    cuentaIBAN = "3456789087456789098890987",
                    idBanco = 1,
                    nombreBanco = "BN",
                    idEstado = 1,
                    nombreEstado = "Activo"
                },
                new EmpleadoDto
                {
                    idEmpleado = 2,
                    nombre = "Juan",
                    segundoNombre = "Carlos",
                    primerApellido = "Pérez",
                    segundoApellido = "Gómez",
                    fechaNacimiento = new DateTime(1990, 5, 15),
                    cedula = 123456789,
                    numeroTelefonico = "8888-8888",
                    correoInstitucional = "juan.perez@example.com",
                    idDireccion = 2,
                    idCargo = 1,
                    nombreCargo = "Administrador",
                    fechaContratacion = new DateTime(2020, 1, 10),
                    fechaSalida = null,
                    periocidadPago = "Mensual",
                    salarioDiario = 100000,
                    salarioAprobado = 3000000,
                    salarioPorMinuto = 0.1,
                    salarioPoHora = 10000,
                    salarioPorHoraExtra = 15000,
                    idMoneda = 1,
                    nombreMoneda = "Colones",
                    cuentaIBAN = "CR123456789012345678",
                    idBanco = 2,
                    nombreBanco = "BCR",
                    idEstado = 1,
                    nombreEstado = "Activo"
                },
                new EmpleadoDto
                {
                    idEmpleado = 3,
                    nombre = "María",
                    segundoNombre = "José",
                    primerApellido = "Rodríguez",
                    segundoApellido = "Castro",
                    fechaNacimiento = new DateTime(1985, 10, 20),
                    cedula = 987654321,
                    numeroTelefonico = "7777-7777",
                    correoInstitucional = "maria.rodriguez@example.com",
                    idDireccion = 3,
                    idCargo = 3,
                    nombreCargo = "Gerente",
                    fechaContratacion = new DateTime(2018, 3, 1),
                    fechaSalida = null,
                    periocidadPago = "Quincenal",
                    salarioDiario = 80000,
                    salarioAprobado = 2400000,
                    salarioPorMinuto = 0.08,
                    salarioPoHora = 8000,
                    salarioPorHoraExtra = 12000,
                    idMoneda = 1,
                    nombreMoneda = "Colones",
                    cuentaIBAN = "CR987654321098765432",
                    idBanco = 3,
                    nombreBanco = "BAC",
                    idEstado = 1,
                    nombreEstado = "Activo"
                }
            };
        }

        #endregion */
    }
}
