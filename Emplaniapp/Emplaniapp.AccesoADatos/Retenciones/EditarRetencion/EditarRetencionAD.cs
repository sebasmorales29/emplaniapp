using System.Data.Entity;
using System.Linq;
using Emplaniapp.Abstracciones.InterfacesAD.Remuneraciones.EditarRemuneracion;
using Emplaniapp.Abstracciones.InterfacesAD.Retenciones;
using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.AccesoADatos.Retenciones
{
    public class EditarRetencionAD : IEditarRetencionAD
    {
        private readonly Contexto _ctx = new Contexto();
        public void Editar(Retencion retencion)
        {
            var e = _ctx.Retenciones.Find(retencion.idRetencion);
            if (e == null) return;
            e.idTipoRetencion = retencion.idTipoRetencion;
            e.rebajo = retencion.rebajo;
            e.fechaRetencion = retencion.fechaRetencion;
            _ctx.SaveChanges();
        }
    }
}