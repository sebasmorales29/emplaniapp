using Emplaniapp.Abstracciones.InterfacesAD.Remuneraciones;
using Emplaniapp.Abstracciones.ModelosParaUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.AccesoADatos.Remuneraciones
{
    public class ListarRemuneracionesAD : IListarRemuneracionesAD
    {
        Contexto contexto;

        public ListarRemuneracionesAD()
        {
            contexto = new Contexto();
        }


        // Para listar solo las remuneraciones de un empleado particular
        public List<RemuneracionDto> Listar(int id)
        {
            List<RemuneracionDto> ListaRem = 
                (from remu in contexto.Remuneracion
                join t_remu in contexto.TipoRemu on remu.idTipoRemuneracion equals t_remu.Id
                join estado in contexto.Estado on remu.idEstado equals estado.idEstado
                select new RemuneracionDto
                {
                    idRemuneracion = remu.idRemuneracion,
                    idEmpleado = remu.idEmpleado,
                    idTipoRemuneracion = remu.idTipoRemuneracion,
                    nombreTipoRemuneracion = t_remu.nombreTipoRemuneracion,
                    fechaRemuneracion = remu.fechaRemuneracion,
                    diasTrabajados = remu.diasTrabajados,
                    horas = remu.horas,
                    comision = remu.comision,
                    pagoQuincenal = remu.pagoQuincenal,
                    idEstado = remu.idEstado,
                    nombreEstado = estado.nombreEstado
                })
                .Where(remunera => remunera.idEmpleado == id)
                .ToList();
            return ListaRem;
        }

        public RemuneracionDto ObtenerPorId(int id)
        {
            var remuneracion = contexto.Remuneracion
                                       .Where(r => r.idRemuneracion == id)
                                       .Select(r => new RemuneracionDto
                                       {
                                           idRemuneracion = r.idRemuneracion,
                                           idEmpleado = r.idEmpleado,
                                           nombreTipoRemuneracion = r.TipoRemuneracion.nombreTipoRemuneracion,
                                           porcentajeRemuneracion = r.TipoRemuneracion.porcentajeRemuneracion,
                                           fechaRemuneracion = r.fechaRemuneracion,
                                           diasTrabajados = r.diasTrabajados,
                                           horas = r.horas,
                                           comision = r.comision,
                                           nombreEstado = r.Estado.nombreEstado
                                       }).FirstOrDefault();
            return remuneracion;
        }
    }
}
