using System;
using System.ComponentModel.DataAnnotations;

namespace Emplaniapp.UI.Models
{
    public class DatosLaboralesViewModel
    {
        public int IdEmpleado { get; set; }

        [Display(Name = "Número de Ocupación")]
        public string NumeroOcupacion { get; set; }

        [Required(ErrorMessage = "El cargo es requerido")]
        [Display(Name = "Cargo")]
        public int? IdCargo { get; set; }
        
        public string Cargo { get; set; }

        [Required(ErrorMessage = "La fecha de ingreso es requerida")]
        [Display(Name = "Fecha de Ingreso")]
        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; }

        [Display(Name = "Fecha de Salida")]
        [DataType(DataType.Date)]
        public DateTime? FechaSalida { get; set; }
    }
} 