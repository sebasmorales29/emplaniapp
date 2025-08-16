using Emplaniapp.Abstracciones.InterfacesAD.Liquidaciones;
using Emplaniapp.Abstracciones.InterfacesParaUI.Liquidaciones;
using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.AccesoADatos.Liquidaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.LogicaDeNegocio.Liquidaciones
{
    public class ObtenerLiqPorEmpleadoIdLN : IObtenerLiqPorEmpleadoIdLN
    {
        IObtenerLiqPorEmpleadoIdAD _obtenerLiq;

        public ObtenerLiqPorEmpleadoIdLN()
        {
            _obtenerLiq = new ObtenerLiqPorEmpleadoIdAD(); 
        }

        public LiquidacionDto ObtenerPorEmpleadoID(int idEmp)
        {
            Liquidacion liqEnBD = _obtenerLiq.ObtenerLiquidacion(idEmp);
            if (liqEnBD == null) { return null; }
            LiquidacionDto liquidacionDto = CambiarADto(liqEnBD);
            return liquidacionDto;
        }

        public LiquidacionDto Activa(int idEmp)
        {
            Liquidacion liqEnBD = _obtenerLiq.LiquidacionActiva(idEmp);
            if (liqEnBD == null) { return null; }
            LiquidacionDto liquidacionDto = CambiarADto(liqEnBD);
            return liquidacionDto;
        }

        private LiquidacionDto CambiarADto(Liquidacion liquid)
        {
            string fechaPrev = "";
            if (liquid.diasPreaviso == 0) { fechaPrev = "No Aplica"; }
            else { fechaPrev = liquid.fechaLiquidacion.AddDays(-liquid.diasPreaviso).ToString(); }

            return new LiquidacionDto
            {
                idLiquidacion = liquid.idLiquidacion,
                idEmpleado = liquid.idEmpleado,
                fechaLiquidacion = liquid.fechaLiquidacion,
                motivoLiquidacion = liquid.motivoLiquidacion,
                salarioPromedio = liquid.salarioPromedio,
                aniosAntiguedad = liquid.aniosAntiguedad,
                diasPreaviso = liquid.diasPreaviso,
                fechaPreaviso = fechaPrev,
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
