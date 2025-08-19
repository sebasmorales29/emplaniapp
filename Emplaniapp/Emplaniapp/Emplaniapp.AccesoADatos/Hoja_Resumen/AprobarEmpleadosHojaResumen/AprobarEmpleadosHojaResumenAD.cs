using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Hoja_Resumen.AprobarEmpleadosHojaResumen;

namespace Emplaniapp.AccesoADatos.Hoja_Resumen.AprobarEmpleadosHojaResumen
{
    public class AprobarEmpleadosHojaResumenAD : IAprobarEmpleadosHojaResumenAD
    {
        private Contexto _contexto;

        public AprobarEmpleadosHojaResumenAD()
        {
            _contexto = new Contexto();
        }

        public bool AprobarPagoQuincenal(int idPagoQuincenal, string idUsuario)
        {
            var pago = _contexto.PagoQuincenal.FirstOrDefault(p => p.idPagoQuincenal == idPagoQuincenal);

            if (pago == null)
                return false;

            pago.aprobacion = true;
            pago.idUsuario = idUsuario;

            _contexto.SaveChanges();
            return true;
        }
    }
}