
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.ModelosParaUI
{
    public class LiquidacionDto
    {
        public int idLiquidacion { get; set; }
        public int idEmpleado { get; set; }

        // Detalles
        [DisplayName("Fecha de Liquidación")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime fechaLiquidacion { get; set; }

        [DisplayName("Motivo")]
        public string motivoLiquidacion { get; set; }

        [DisplayName("Salario Promedio")]
        public decimal salarioPromedio { get; set; }

        [DisplayName("Años Trabajados")]
        public int aniosAntiguedad { get; set; }

        [DisplayName("Días de preaviso")]
        public int diasPreaviso { get; set; }

        [DisplayName("Fecha de Preaviso")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public string fechaPreaviso { get; set; }

        [DisplayName("Vacaciones Pendientes")]
        public int diasVacacionesPendientes { get; set; }



        // Cálculo
        [DisplayName("Preaviso")]
        public decimal pagoPreaviso { get; set; }

        [DisplayName("Aguinaldo Proporcional")]
        public decimal pagoAguinaldoProp { get; set; }

        [DisplayName("Cesantía")]
        public decimal pagoCesantia { get; set; }

        [DisplayName("Vacaciones No Gozadas")]
        public decimal pagoVacacionesNG { get; set; }

        [DisplayName("Remuneraciones Pendientes")]
        public decimal remuPendientes { get; set; }

        [DisplayName("Total")]
        public decimal costoLiquidacion { get; set; }



        [DisplayName("Observaciones")]
        public string observacionLiquidacion { get; set; }
        public int idEstado { get; set; }

    }
}
