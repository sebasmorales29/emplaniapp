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
        IMostrarCalculosLiqLN _calculosPrevios;

        public EditarLiquidacionLN() 
        { 
            _editarLiq = new EditarLiquidacionAD();
            _calculosPrevios = new MostrarCalculosLiqLN();
        }

        // Edición de datos generales ------------------------------------------------------------
        public int Editar(LiquidacionDto liq)
        {
            int seActualizaLiq = _editarLiq.Editar(CambioBD(liq));
            return seActualizaLiq;
        }

        private Liquidacion CambioBD(LiquidacionDto liquid)
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


        // Aprobar la liquidación (cambio de estado a 1: Activo) ----------------------------------
        public int EditarFinal(LiquidacionDto liquidacion)
        {
            int seActualizaLiq = _editarLiq.Editar(CambioEstado(liquidacion));
            return seActualizaLiq;
        }

        private Liquidacion CambioEstado(LiquidacionDto liquid)
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
                idEstado = 1 // Que está activa -> Se guarda
            };
        }


    }
}
