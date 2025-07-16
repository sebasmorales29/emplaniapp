using System.Linq;
using Emplaniapp.Abstracciones.InterfacesAD.Remuneraciones.ObtenerRemuneracionPorId;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.AccesoADatos.Remuneraciones.ObtenerRemuneracionPorId
{
    public class ObtenerRemuneracionPorIdAD : IObtenerRemuneracionPorIdAD
    {
        private Contexto _contexto;

        public ObtenerRemuneracionPorIdAD()
        {
            _contexto = new Contexto();
        }

        public RemuneracionDto ObtenerPorId(int idRemuneracion)
        {
            var remuneracion = _contexto.Remuneracion
                .Where(r => r.idRemuneracion == idRemuneracion)
                .Select(r => new RemuneracionDto
                {
                    idRemuneracion = r.idRemuneracion,
                    idTipoRemuneracion = r.idTipoRemuneracion,
                    nombreTipoRemuneracion = r.TipoRemuneracion.nombreTipoRemuneracion,
                    fechaRemuneracion = r.fechaRemuneracion,
                    horasTrabajadas = r.horasTrabajadas,
                    horasExtras = r.horasExtras,
                    comision = r.comision,
                    pagoQuincenal = r.pagoQuincenal,
                    horasFeriados = r.horasFeriados,
                    horasVacaciones = r.horasVacaciones,
                    horasLicencias = r.horasLicencias,
                    nombreEstado = r.Estado.nombreEstado 
                })
                .FirstOrDefault();

            return remuneracion;
        }
    }
}
