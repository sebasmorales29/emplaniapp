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
    public class EditarTipoRemuneracionAD : IEditarTipoRemuneracionAD
    {

        Contexto contexto;

        public EditarTipoRemuneracionAD()
        {
            contexto = new Contexto();
        }

        public int Editar(TipoRemuneracion tRemuAEditar)
        {
            TipoRemuneracion tipoRemu = contexto.TipoRemu.
                         Where(tremu => tremu.Id == tRemuAEditar.Id).FirstOrDefault();

            // Los valores a modificar
            tipoRemu.Id = tRemuAEditar.Id;
            tipoRemu.nombreTipoRemuneracion = tRemuAEditar.nombreTipoRemuneracion;
            tipoRemu.porcentajeRemuneracion = tRemuAEditar.porcentajeRemuneracion;
            tipoRemu.idEstado = tRemuAEditar.idEstado;

            EntityState estado = contexto.Entry(tipoRemu).State = System.Data.Entity.EntityState.Modified;
            int seEditoTRemu = contexto.SaveChanges();
            return seEditoTRemu;
        }

    }
}
