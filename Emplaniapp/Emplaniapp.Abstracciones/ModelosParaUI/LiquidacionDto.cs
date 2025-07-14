
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public DateTime fechaLiquidacion { get; set; }

        [DisplayName("Motivo")]
        public string motivoLiquidacion { get; set; }

        [DisplayName("Salario Promedio")]
        public decimal salarioPromedio { get; set; }

        [DisplayName("Años de Antigüedad en la empresa")]
        public int aniosAntiguedad { get; set; }

        [DisplayName("Días de preaviso")]
        public int diasPreaviso { get; set; }

        [DisplayName("Vacaciones Pendientes")]
        public int diasVacacionesPendientes { get; set; }



        // Cálculo
        [DisplayName("Preaviso")]
        public decimal pagoPreaviso { get; set; }

        [DisplayName("Aguinaldo Proporcional")]
        public decimal pagoAguinaldoProp { get; set; }

        [DisplayName("Cesantía")]
        public decimal pagoCesantia { get; set; }

        [DisplayName("Remuneraciones Pendientes")]
        public decimal remuPendientes { get; set; }

        [DisplayName("Retenciones Pendientes")]
        public decimal deducPendientes { get; set; }

        [DisplayName("Total")]
        public decimal costoLiquidacion { get; set; }


        public string observacionLiquidacion { get; set; }
        public int idEstado { get; set; }

    }
}
