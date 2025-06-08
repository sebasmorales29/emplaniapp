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
    public class EliminarTipoRetencionAD : IEliminarTipoRetencionAD
    {

        Contexto contexto;

        public EliminarTipoRetencionAD()
        {
            contexto = new Contexto();
        }

        
        public int Eliminar(int id)
        {
            TipoRetencion tipoReten = contexto.TipoReten.Where(tret => tret.Id == id).FirstOrDefault();
            contexto.TipoReten.Remove(tipoReten);
            EntityState estado = contexto.Entry(tipoReten).State = System.Data.Entity.EntityState.Deleted;
            int seEliminoTipoRet = contexto.SaveChanges();
            return seEliminoTipoRet;
        }
         

    }
}
