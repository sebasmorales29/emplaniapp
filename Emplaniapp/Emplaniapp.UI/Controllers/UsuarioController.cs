using Emplaniapp.Abstracciones.Entidades;
using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.AccesoADatos;
using Emplaniapp.LogicaDeNegocio.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Emplaniapp.UI.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuarioController : Controller
    {
        private readonly IObtenerEmpleadoPorIdLN _obtenerEmpleadoLN;
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationRoleManager _roleManager;

        public UsuarioController()
        {
            _obtenerEmpleadoLN = new ObtenerEmpleadoPorIdLN();
            var userStore = new UserStore<ApplicationUser>(new Contexto());
            var roleStore = new RoleStore<IdentityRole>(new Contexto());
            _userManager = new ApplicationUserManager(userStore);
            _roleManager = new ApplicationRoleManager(roleStore);
        }

        // GET: Usuario/RolesYPermisos/5
        public async Task<ActionResult> RolesYPermisos(int id)
        {
            var empleado = _obtenerEmpleadoLN.ObtenerEmpleadoPorId(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }

            var user = await _userManager.FindByIdAsync(empleado.IdNetUser);
            if (user == null)
            {
                TempData["Mensaje"] = $"Error de integridad: El empleado '{empleado.nombre} {empleado.primerApellido}' tiene una referencia a un usuario del sistema que no existe (ID: {empleado.IdNetUser}).";
                TempData["TipoMensaje"] = "error";
                return RedirectToAction("Detalles", "DatosPersonales", new { id = empleado.idEmpleado });
            }

            var userRoles = await _userManager.GetRolesAsync(user.Id);

            var model = new RolesYPermisosViewModel
            {
                IdEmpleado = empleado.idEmpleado,
                NombreCompleto = $"{empleado.nombre} {empleado.primerApellido} {empleado.segundoApellido}",
                UserName = user.UserName,
                Roles = _roleManager.Roles.ToList().Select(r => new SelectListItem
                {
                    Selected = userRoles.Contains(r.Name),
                    Text = r.Name,
                    Value = r.Name
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RolesYPermisos(RolesYPermisosViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return HttpNotFound("No se encontró el usuario.");
            }

            var userRoles = await _userManager.GetRolesAsync(user.Id);
            var selectedRoles = model.Roles.Where(r => r.Selected).Select(r => r.Value).ToList();

            var resultAdd = await _userManager.AddToRolesAsync(user.Id, selectedRoles.Except(userRoles).ToArray());

            if (!resultAdd.Succeeded)
            {
                ModelState.AddModelError("", "Error al añadir roles.");
                return View(model);
            }

            var resultRemove = await _userManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRoles).ToArray());

            if (!resultRemove.Succeeded)
            {
                ModelState.AddModelError("", "Error al remover roles.");
                return View(model);
            }

            TempData["Mensaje"] = "Los roles se han actualizado correctamente.";
            TempData["TipoMensaje"] = "success";

            return RedirectToAction("Detalles", "DatosPersonales", new { id = model.IdEmpleado });
        }
    }
} 