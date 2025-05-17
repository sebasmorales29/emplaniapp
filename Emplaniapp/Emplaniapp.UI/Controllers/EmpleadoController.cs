using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.UI.Controllers
{
    public class EmpleadoController : Controller
    {
        // GET: Empleado
        public ActionResult ListarEmpleados()
        {
            // Crear una lista de empleados (datos de ejemplo)
            var empleados = new List<EmpleadoDto>
            {
                new EmpleadoDto
                {
                    idEmpleado = 1,
                    nombre = "Juan",
                    segundoNombre = "Carlos",
                    primerApellido = "Pérez",
                    segundoApellido = "Gómez",
                    fechaNacimiento = new DateTime(1990, 5, 15),
                    cedula = 123456789,
                    numeroTelefonico = "88888888",
                    correoInstitucional = "juan.perez@example.com",
                    idDireccion = 1,
                    idCargo = 1,
                    nombreCargo = "Administrador",
                    fechaContratacion = new DateTime(2020, 1, 10),
                    fechaSalida = null,
                    periocidadPago = "Mensual",
                    salarioDiario = 100,
                    salarioAprobado = 3000,
                    salarioPorMinuto = 0.1,
                    salarioPoHora = 10,
                    salarioPorHoraExtra = 15,
                    idMoneda = 1,
                    nombreMoneda = "USD",
                    cuentaIBAN = "CR123456789012345678",
                    idBanco = 1,
                    nombreBanco = "Banco Ejemplo",
                    idEstado = 1,
                    nombreEstado = "Activo"
                },
                new EmpleadoDto
                {
                    idEmpleado = 2,
                    nombre = "María",
                    segundoNombre = "José",
                    primerApellido = "Rodríguez",
                    segundoApellido = "Castro",
                    fechaNacimiento = new DateTime(1985, 10, 20),
                    cedula = 987654321,
                    numeroTelefonico = "77777777",
                    correoInstitucional = "maria.rodriguez@example.com",
                    idDireccion = 2,
                    idCargo = 2,
                    nombreCargo = "Contador",
                    fechaContratacion = new DateTime(2018, 3, 1),
                    fechaSalida = null,
                    periocidadPago = "Quincenal",
                    salarioDiario = 80,
                    salarioAprobado = 2400,
                    salarioPorMinuto = 0.08,
                    salarioPoHora = 8,
                    salarioPorHoraExtra = 12,
                    idMoneda = 1,
                    nombreMoneda = "USD",
                    cuentaIBAN = "CR987654321098765432",
                    idBanco = 2,
                    nombreBanco = "Banco Otro",
                    idEstado = 1,
                    nombreEstado = "Activo"
                },
                 new EmpleadoDto
                {
                    idEmpleado = 3,
                    nombre = "Carlos",
                    segundoNombre = "Andres",
                    primerApellido = "Ruiz",
                    segundoApellido = "Arauz",
                    fechaNacimiento = new DateTime(1993, 02, 01),
                    cedula = 54645646,
                    numeroTelefonico = "88565656",
                    correoInstitucional = "carlos.Ruiz@example.com",
                    idDireccion = 3,
                    idCargo = 3,
                    nombreCargo = "Bodeguero",
                    fechaContratacion = new DateTime(2022, 03, 01),
                    fechaSalida = null,
                    periocidadPago = "Quincenal",
                    salarioDiario = 90,
                    salarioAprobado = 2700,
                    salarioPorMinuto = 0.9,
                    salarioPoHora = 9,
                    salarioPorHoraExtra = 13,
                    idMoneda = 1,
                    nombreMoneda = "USD",
                    cuentaIBAN = "CR987654321098765432",
                    idBanco = 3,
                    nombreBanco = "Banco Otro",
                    idEstado = 1,
                    nombreEstado = "Activo"
                }
            };
            return View(empleados);
        }

        // GET: Empleado/Details/5
        public ActionResult DetalleEmpleado(int id)
        {
            return View();
        }

        // GET: Empleado/Create
        public ActionResult CrearEmpleado()
        {
            return View();
        }

        // POST: Empleado/Create
        [HttpPost]
        public ActionResult CrearEmpleado(FormCollection collection)
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

        // GET: Empleado/Edit/5
        public ActionResult EditarEmpleado(int id)
        {
            return View();
        }

        // POST: Empleado/Edit/5
        [HttpPost]
        public ActionResult EditarEmpleado(int id, FormCollection collection)
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

        // GET: Empleado/Delete/5
        public ActionResult EliminarEmpleado(int id)
        {
            return View();
        }

        // POST: Empleado/Delete/5
        [HttpPost]
        public ActionResult EliminarEmpleado(int id, FormCollection collection)
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
