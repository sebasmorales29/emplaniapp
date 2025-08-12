using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.PeriodoPago;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.AccesoADatos.PeriodoPago
{
    public class CrearPeriodoPagoAD : ICrearPeriodoPagoAD
    {
        private Contexto _contexto;


        public CrearPeriodoPagoAD()
        {
            _contexto = new Contexto();
        }

        public List<PeriodoPagoDto> GenerarPeriodoPago(DateTime? fechaProceso = null)
        {
            try
            {
                var fechaParam = fechaProceso.HasValue
                    ? new System.Data.SqlClient.SqlParameter("@fechaCreacion", fechaProceso.Value)
                    : new System.Data.SqlClient.SqlParameter("@fechaCreacion", DBNull.Value);

                var resultado = _contexto.Database.SqlQuery<PeriodoPagoDto>(
                    "EXEC GenerarPeriodoPagoAutomatico @fechaCreacion",
                    fechaParam
                ).ToList();

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar periodo de pago: " + ex.Message);
            }
        }

    }
}
