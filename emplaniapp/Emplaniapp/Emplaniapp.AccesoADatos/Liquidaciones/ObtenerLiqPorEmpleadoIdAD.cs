using Emplaniapp.Abstracciones.InterfacesAD.Liquidaciones;
using Emplaniapp.Abstracciones.ModelosAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.AccesoADatos.Liquidaciones
{
    public class ObtenerLiqPorEmpleadoIdAD : IObtenerLiqPorEmpleadoIdAD
    {
        Contexto contexto;

        public ObtenerLiqPorEmpleadoIdAD()
        {
            contexto = new Contexto();
        }

        public Liquidacion ObtenerLiquidacion (int idEmpleado)
        {
            Liquidacion liq = contexto.Liquidaciones.
                Where(liquid => liquid.idEmpleado == idEmpleado).FirstOrDefault();
            return liq;
        }

        public Liquidacion LiquidacionActiva(int idEmpleado)
        {
            Liquidacion liq = contexto.Liquidaciones.
                Where(liquid => liquid.idEmpleado == idEmpleado 
                && liquid.idEstado == 1).FirstOrDefault();
            return liq;
        }



    }
}
