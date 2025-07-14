using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.Abstracciones.InterfacesParaUI.Liquidaciones;
using Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.LogicaDeNegocio.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.LogicaDeNegocio.Remuneraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.LogicaDeNegocio.Liquidaciones
{
    public class MostrarCalculosPreviosLiqLN : IMostrarCalculosPreviosLiqLN
    {
        IObtenerEmpleadoPorIdLN _empleado;
        IListarRemuneracionesLN _remuneraciones;
        

        public MostrarCalculosPreviosLiqLN() { 
          _empleado = new ObtenerEmpleadoPorIdLN();
          _remuneraciones = new ListarRemuneracionesLN();
        }

        public LiquidacionDto MostrarLiquidacion(int id)
        {
            LiquidacionDto liquid = new LiquidacionDto
            {
                idLiquidacion = 1,
                idEmpleado = _empleado.ObtenerEmpleadoPorId(id).idEmpleado,

                fechaLiquidacion = DateTime.Now,
                motivoLiquidacion = "renuncia",
                salarioPromedio = 125000,
                aniosAntiguedad = DateTime.Now.Year - _empleado.ObtenerEmpleadoPorId(id).fechaContratacion.Year,
                diasPreaviso = 30,
                diasVacacionesPendientes = 4,

                pagoPreaviso = _empleado.ObtenerEmpleadoPorId(id).salarioAprobado, // 
                pagoAguinaldoProp = 125000*3,
                pagoCesantia = 0,
                remuPendientes = 399002,
                deducPendientes = 0,
                costoLiquidacion = 23456789,
               
                observacionLiquidacion = "",
                idEstado = 1
            }; 

            return liquid;
        }


        
        



    }
}
