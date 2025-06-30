using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Emplaniapp.UI.Models
{
    public class EditarRetencionViewModel
    {
        public int IdRetencion { get; set; }
        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        [Required]
        public int IdTipoRetencion { get; set; }
        public decimal Porcentaje { get; set; }
        [DataType(DataType.Currency)]
        public decimal MontoRetencion { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaRetencion { get; set; }
        public IEnumerable<SelectListItem> TiposRetencion { get; set; }
    }
}