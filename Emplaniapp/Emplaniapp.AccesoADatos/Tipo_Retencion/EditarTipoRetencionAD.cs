using Emplaniapp.Abstracciones.InterfacesAD.Tipo_Retencion;
using Emplaniapp.Abstracciones.ModelosAD;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.AccesoADatos.Tipo_Retencion
{
    public class EditarTipoRetencionAD : IEditarTipoRetencionAD
    {

        Contexto contexto;

        public EditarTipoRetencionAD()
        {
            contexto = new Contexto();
        }


     
         public int Editar(TipoRetencion tRetenAEditar)
        {
            TipoRetencion tipoReten = contexto.TipoReten.
                         Where(tret => tret.Id == tRetenAEditar.Id).FirstOrDefault();

            tipoReten.Id = tRetenAEditar.Id;
            tipoReten.nombreTipoRetencion = tRetenAEditar.nombreTipoRetencion;
            tipoReten.porcentajeRetencion = tRetenAEditar.porcentajeRetencion;
            tipoReten.idEstado = tRetenAEditar.idEstado;

            EntityState estado = contexto.Entry(tipoReten).State = System.Data.Entity.EntityState.Modified;
            int seEditoTReten = contexto.SaveChanges();
            return seEditoTReten;
        }
        

    }
}
