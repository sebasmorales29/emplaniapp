using Emplaniapp.Abstracciones.InterfacesAD.Tipo_Remuneracion;
using Emplaniapp.Abstracciones.ModelosAD;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.AccesoADatos.Tipo_Remuneracion
{
    public class AgregarTipoRemuneracionAD : IAgregarTipoRemuneracionAD
    {

        Contexto contexto;

        public AgregarTipoRemuneracionAD()
        {
            contexto = new Contexto();
        }


        public async Task<int> Agregar(TipoRemuneracion TRemu) 
        {
            TRemu.idEstado = 1; // Se establece el estado como "Activo" por defecto
            contexto.TipoRemu.Add(TRemu); 
            EntityState estado = contexto.Entry(TRemu).State = System.Data.Entity.EntityState.Added;
            int tipoRemuGuardado = await contexto.SaveChangesAsync();
            return tipoRemuGuardado;
        }

        


    }
}
