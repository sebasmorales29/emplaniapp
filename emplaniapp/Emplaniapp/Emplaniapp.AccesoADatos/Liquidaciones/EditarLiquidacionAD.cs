using Emplaniapp.Abstracciones.InterfacesAD.Liquidaciones;
using Emplaniapp.Abstracciones.ModelosAD;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.AccesoADatos.Liquidaciones
{
    public class EditarLiquidacionAD : IEditarLiquidacionAD
    {
        Contexto contexto;

        public EditarLiquidacionAD()
        {
            contexto = new Contexto();
        }

        public int Editar(Liquidacion liquid)
        {
            Liquidacion liqEdit = contexto.Liquidaciones.
                Where(l => l.idLiquidacion == liquid.idLiquidacion).
                FirstOrDefault();

            liqEdit.idLiquidacion = liquid.idLiquidacion;
            liqEdit.idEmpleado = liquid.idEmpleado;
            liqEdit.fechaLiquidacion = liquid.fechaLiquidacion;
            liqEdit.motivoLiquidacion = liquid.motivoLiquidacion;
            liqEdit.salarioPromedio = liquid.salarioPromedio;
            liqEdit.aniosAntiguedad = liquid.aniosAntiguedad;
            liqEdit.diasPreaviso = liquid.diasPreaviso;
            liqEdit.diasVacacionesPendientes = liquid.diasVacacionesPendientes;
            liqEdit.pagoPreaviso = liquid.pagoPreaviso;
            liqEdit.pagoAguinaldoProp = liquid.pagoAguinaldoProp;
            liqEdit.pagoCesantia = liquid.pagoCesantia;
            liqEdit.remuPendientes = liquid.remuPendientes;
            liqEdit.costoLiquidacion = liquid.costoLiquidacion;
            liqEdit.observacionLiquidacion = liquid.observacionLiquidacion;
            liqEdit.idEstado = liquid.idEstado;

            EntityState estado = contexto.Entry(liqEdit).State = System.Data.Entity.EntityState.Modified;
            int seEditoLiq = contexto.SaveChanges();
            return seEditoLiq;
        }

    }
}
