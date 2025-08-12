using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.PeriodoPago;
using Emplaniapp.Abstracciones.InterfacesParaUI.PeriodoPago.CrearPeriodoPago;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.PeriodoPago;

namespace Emplaniapp.LogicaDeNegocio.PeriodoPago
{
    public class CrearPeriodoPagoLN : ICrearPeriodoPagoLN
    {
        private ICrearPeriodoPagoAD _crearPeriodoPagoAD;

        public CrearPeriodoPagoLN()
        {
            _crearPeriodoPagoAD = new CrearPeriodoPagoAD();
        }
        public List<PeriodoPagoDto> GenerarPeriodoPago(DateTime? fechaProceso = null)
        {
            try
            {
                return _crearPeriodoPagoAD.GenerarPeriodoPago(fechaProceso);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar el periodo de pago quincenal: " + ex.Message);
            }
        }
    }
}
