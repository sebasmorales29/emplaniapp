using System.Collections.Generic;
using System.Web.Mvc;

namespace Emplaniapp.UI.Models
{
    public class RolesYPermisosViewModel
    {
        public int IdEmpleado { get; set; }
        public string NombreCompleto { get; set; }
        public string UserName { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }
} 