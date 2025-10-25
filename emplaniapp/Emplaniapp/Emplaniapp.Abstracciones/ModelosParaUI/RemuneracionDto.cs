using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosParaUI
{
    public class RemuneracionDto
    {
        public int idRemuneracion { get; set; }
        public int idEmpleado { get; set; }
        public int idTipoRemuneracion { get; set; }

        [DisplayName("Tipo de Remuneración")]
        public string nombreTipoRemuneracion { get; set; }

        [DisplayName("Fecha de Remuneración")]
        public DateTime fechaRemuneracion { get; set; }

        [DisplayName("Días trabajados")]
        public int? diasTrabajados { get; set; }

        [DisplayName("Horas trabajadas")]
        public int? horas { get; set; }

        [DisplayName("Total de Comisión")]
        public decimal? comision { get; set; }

        [DisplayName("Pago Quincenal")]
        public decimal? pagoQuincenal { get; set; }        
        public int idEstado { get; set; }

        [DisplayName("Estado")]
        public string nombreEstado { get; set; }

        [DisplayName("Porcentaje de remuneración")]
        public double? porcentajeRemuneracion { get; set; }

        [DisplayName("Día Feriado")]
        public bool TrabajoEnDia { get; set; }

        [DisplayName("Salario por Hora")]
        public decimal SalarioPorHoraExtra { get; set; }

        [DisplayName("Salario por Día")]
        public decimal SalarioDiario { get; set; }
    }
}
