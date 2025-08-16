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
    public class EditarLiquidacionLN : IEditarLiquidacionLN
    {
        IEditarLiquidacionAD _editarLiq;
        IMostrarCalculosPreviosLiqLN _calculosPrevios;

        public EditarLiquidacionLN() 
        { 
            _editarLiq = new EditarLiquidacionAD();
            _calculosPrevios = new MostrarCalculosPreviosLiqLN();
        }

        public int Editar(EmpleadoDto emp, LiquidacionDto liquidacion)
        {
            int seActualizaLiq = _editarLiq.Editar(CambioBD(emp,liquidacion));
            return seActualizaLiq;
        }

        private Liquidacion CambioBD(EmpleadoDto emp, LiquidacionDto liq)
        {
            LiquidacionDto liquid = _calculosPrevios.MostrarLiquidacionTotal
                (emp, liq.fechaLiquidacion, liq.motivoLiquidacion);
            return new Liquidacion
            {
                idLiquidacion = liq.idLiquidacion,
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



        public int EditarFinal(EmpleadoDto emp, LiquidacionDto liquidacion)
        {
            int seActualizaLiq = _editarLiq.Editar(CambioEstado(emp, liquidacion));
            return seActualizaLiq;
        }

        private Liquidacion CambioEstado(EmpleadoDto emp, LiquidacionDto liq)
        {
            LiquidacionDto liquid = _calculosPrevios.MostrarLiquidacionTotal
                (emp, liq.fechaLiquidacion, liq.motivoLiquidacion);
            return new Liquidacion
            {
                idLiquidacion = liq.idLiquidacion,
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
                idEstado = 1
            };
        }


    }
}
