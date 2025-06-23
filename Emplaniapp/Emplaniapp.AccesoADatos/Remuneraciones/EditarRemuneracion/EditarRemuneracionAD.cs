using System.Data.Entity;
using System.Linq;
using Emplaniapp.Abstracciones.InterfacesAD.Remuneraciones.EditarRemuneracion;
using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.AccesoADatos.Remuneraciones.EditarRemuneracion
{
    public class EditarRemuneracionAD : IEditarRemuneracionAD
    {
        private Contexto _contexto;

        public EditarRemuneracionAD()
        {
            _contexto = new Contexto();
        }

        public int Actualizar(RemuneracionDto laRemuneracion)
        {
            Remuneracion laRemuneracionEnBaseDeDatos = _contexto.Remuneracion.Where(Remuneracion => Remuneracion.idRemuneracion == laRemuneracion.idRemuneracion).FirstOrDefault();
            laRemuneracionEnBaseDeDatos.horasExtras = laRemuneracion.horasExtras;
            laRemuneracionEnBaseDeDatos.horasTrabajadas = laRemuneracion.horasTrabajadas;
            laRemuneracionEnBaseDeDatos.comision = laRemuneracion.comision;
            laRemuneracionEnBaseDeDatos.pagoQuincenal = laRemuneracion.pagoQuincenal;
            laRemuneracionEnBaseDeDatos.horasFeriados = laRemuneracion.horasFeriados;
            laRemuneracionEnBaseDeDatos.horasVacaciones = laRemuneracion.horasVacaciones;
            laRemuneracionEnBaseDeDatos.horasLicencias = laRemuneracion.horasLicencias;
            EntityState estado = _contexto.Entry(laRemuneracionEnBaseDeDatos).State = System.Data.Entity.EntityState.Modified;
            int cantidadDatosAgregados = _contexto.SaveChanges();
            return cantidadDatosAgregados;
        }
    }
}
