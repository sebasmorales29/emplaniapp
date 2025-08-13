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
                    diasTrabajados = r.diasTrabajados,
                    horas = r.horas,
                    comision = r.comision,
                    pagoQuincenal = r.pagoQuincenal,
                    nombreEstado = r.Estado.nombreEstado,

                    SalarioDiario = r.Empleado.salarioDiario, 
                    SalarioPorHoraExtra = r.Empleado.salarioPorHoraExtra
                })
                .FirstOrDefault();

            return remuneracion;
        }
    }
}
