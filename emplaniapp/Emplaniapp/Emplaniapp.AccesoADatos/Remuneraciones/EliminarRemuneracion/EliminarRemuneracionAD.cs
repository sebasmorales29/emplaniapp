using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Remuneraciones.EliminarRemuneracion;

namespace Emplaniapp.AccesoADatos.Remuneraciones.EliminarRemuneracion
{
    public class EliminarRemuneracionAD : IEliminarRemuneracionAD
    {
        private Contexto _contexto;

        public EliminarRemuneracionAD()
        {
            _contexto = new Contexto();
        }

        public bool EliminarRemuneracion(int idRemuneracion)
        {
            var remuneracion = _contexto.Remuneracion.Find(idRemuneracion);
            if (remuneracion == null)
                return false;

            var pagosQuincenales = _contexto.PagoQuincenal
                .Where(p => p.idRemuneracion == idRemuneracion)
                .ToList();

            if (pagosQuincenales.Any())
            {
                _contexto.PagoQuincenal.RemoveRange(pagosQuincenales);
            }

            _contexto.Remuneracion.Remove(remuneracion);

            _contexto.SaveChanges();

            return true;
        }
    }
}
