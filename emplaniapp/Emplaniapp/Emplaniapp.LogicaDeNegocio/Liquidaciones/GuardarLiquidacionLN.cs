using Emplaniapp.Abstracciones.InterfacesAD.Liquidaciones;
using Emplaniapp.Abstracciones.InterfacesParaUI.Liquidaciones;
using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Liquidaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.LogicaDeNegocio.Liquidaciones
{
    public class GuardarLiquidacionLN : IGuardarLiquidacionLN
    {
        IGuardarLiquidacionAD _guardarLiq;
        

        public GuardarLiquidacionLN()
        {
            _guardarLiq = new GuardarLiquidacionAD();
        }


        public async Task<int> Guardar(LiquidacionDto liquid) // Para guardar un archivo
        {
            int seGuardoLiq = await _guardarLiq.Guardar(ObtenerLiq(liquid));
            return seGuardoLiq;
        }

        private Liquidacion ObtenerLiq(LiquidacionDto liquid)
        {
            return new Liquidacion
            {
                idLiquidacion = liquid.idLiquidacion,
                idEmpleado = liquid.idEmpleado,
                fechaLiquidacion = liquid.fechaLiquidacion,
                motivoLiquidacion = liquid.motivoLiquidacion,
                salarioPromedio = liquid.salarioPromedio,
                aniosAntiguedad = liquid.aniosAntiguedad,
                diasPreaviso = liquid.diasPreaviso,
                diasVacacionesPendientes = liquid.diasVacacionesPendientes,
                pagoPreaviso = liquid.pagoPreaviso,
                pagoAguinaldoProp = liquid.pagoAguinaldoProp,
                pagoCesantia = liquid.pagoCesantia,
                remuPendientes = liquid.remuPendientes,
                costoLiquidacion = liquid.costoLiquidacion,
                observacionLiquidacion = liquid.observacionLiquidacion,
                idEstado = liquid.idEstado
            };
        }


    }
}
