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
    public class EliminarTipoRemuneracionAD : IEliminarTipoRemuneracionAD
    {
        Contexto contexto;

        public EliminarTipoRemuneracionAD()
        {
            contexto = new Contexto();
        }

        public int Eliminar(int id)
        {
            TipoRemuneracion tipoRemu = contexto.TipoRemu.Where(tremu => tremu.Id == id).FirstOrDefault();
            contexto.TipoRemu.Remove(tipoRemu);
            EntityState estado = contexto.Entry(tipoRemu).State = System.Data.Entity.EntityState.Deleted;
            int seEliminoTipoRemu = contexto.SaveChanges();
            return seEliminoTipoRemu;
        }

    }
}
