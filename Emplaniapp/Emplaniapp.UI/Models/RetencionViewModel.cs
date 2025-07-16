// Emplaniapp.UI/Models/RetencionViewModel.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Emplaniapp.UI.Models
{
    public class RetencionViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int IdRetencion { get; set; }

        [Required]
        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }

        [DataType(DataType.Currency)]
        public decimal SalarioBase { get; set; }

        [Required]
        public int IdTipoRetencion { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "El porcentaje debe estar entre 0 y 100.")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Porcentaje { get; set; }

        [DataType(DataType.Currency)]
        public decimal MontoRetencion { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime FechaRetencion { get; set; }

        public IEnumerable<SelectListItem> TiposRetencion { get; set; }
    }
}
