using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Remuneraciones.EliminarRemuneracion;
using Emplaniapp.Abstracciones.InterfacesAD.Retenciones;

namespace Emplaniapp.AccesoADatos.Retenciones
{
    public class EliminarRetencionAD : IEliminarRetencionAD
    {
        private readonly Contexto _ctx = new Contexto();
        public void Eliminar(int idRetencion)
        {
            var e = _ctx.Retenciones.Find(idRetencion);
            if (e == null) return;
            _ctx.Retenciones.Remove(e);
            _ctx.SaveChanges();
        }
    }
}