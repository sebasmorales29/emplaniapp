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
    public class AgregarTipoRetencionAD : IAgregarTipoRetencionAD
    {

        Contexto contexto;

        public AgregarTipoRetencionAD()
        {
            contexto = new Contexto();
        }


        public async Task<int> Agregar(TipoRetencion TReten) 
        {
            TReten.idEstado = 1; // Se establece el estado como "Activo" por defecto
            contexto.TipoReten.Add(TReten); 
            EntityState estado = contexto.Entry(TReten).State = System.Data.Entity.EntityState.Added;
            int tipoRetenGuardado = await contexto.SaveChangesAsync();
            return tipoRetenGuardado;
        }

        
         
    }
}
