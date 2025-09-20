using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.Abstracciones.InterfacesParaUI.Liquidaciones;
using Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones;
using Emplaniapp.Abstracciones.InterfacesParaUI.Tipo_Remuneracion;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.LogicaDeNegocio.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.LogicaDeNegocio.Remuneraciones;
using Emplaniapp.LogicaDeNegocio.Tipo_Remuneracion;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.EntitySql;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.LogicaDeNegocio.Liquidaciones
{
    public class MostrarCalculosLiqLN : IMostrarCalculosLiqLN
    {
        IObtenerEmpleadoPorIdLN _empleado;
        IGenerarCalculoLiqLN _generarCalculo;


        public MostrarCalculosLiqLN()
        {
            _empleado = new ObtenerEmpleadoPorIdLN();
            _generarCalculo = new GenerarCalculoLiqLN();
        }


        public LiquidacionDto MostrarLiquidacionParcial(EmpleadoDto emp)
        {
            // Datos generales
            DateTime fechaliq = DateTime.Now.Date;

            // Generar objero liquidación previo
            LiquidacionDto liquid = new LiquidacionDto
            {
                idEmpleado = emp.idEmpleado,

                // Lo que se puede editar
                fechaLiquidacion = fechaliq,
                motivoLiquidacion = "",
                // Lo que se toma en cuenta
                salarioPromedio = 0,
                aniosAntiguedad = 0,
                diasPreaviso = 0,
                fechaPreaviso = "--/--/----",
                diasVacacionesPendientes = 0,

                // Cálculos finales
                pagoPreaviso = 0,
                pagoAguinaldoProp = 0,
                pagoCesantia = 0,
                remuPendientes = 0,
                costoLiquidacion = 0,

                observacionLiquidacion = "",
                idEstado = 0
            };

            return liquid;
        }

        public LiquidacionDto MostrarLiquidacionTotal(int caso, LiquidacionDto liq) {

            LiquidacionDto liquid = new LiquidacionDto();

            if (caso == 1) { // Si se crea por primera vez
                EmpleadoDto emp = _empleado.ObtenerEmpleadoPorId(liq.idEmpleado);
                liquid = _generarCalculo.PrimerCalculo
                    (emp, liq.fechaLiquidacion,liq.motivoLiquidacion, liq.observacionLiquidacion);
            }
            else {  // Si es editar
                liquid = _generarCalculo.ModificarCalculo(liq);
            }

            return liquid;
            
        }


    }
}
