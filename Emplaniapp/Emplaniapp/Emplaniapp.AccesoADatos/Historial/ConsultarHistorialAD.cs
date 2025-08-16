using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Historial;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.Abstracciones.ModelosAD;

namespace Emplaniapp.AccesoADatos.Historial
{
    public class ConsultarHistorialAD : IConsultarHistorial
    {
        public List<HistorialEmpleadoDto> ObtenerHistorialEmpleado(int idEmpleado, int? idTipoEvento = null, DateTime? fechaInicio = null, DateTime? fechaFin = null, string categoriaEvento = null, int top = 100)
        {
            try
            {
                using (var contexto = new Contexto())
                {
                    // Si no se especifica fecha fin, usar la actual
                    if (!fechaFin.HasValue)
                        fechaFin = DateTime.Now;
                    
                    // Si no se especifica fecha inicio, usar último mes
                    if (!fechaInicio.HasValue)
                        fechaInicio = fechaFin.Value.AddMonths(-1);

                    var query = from h in contexto.HistorialEmpleado
                               join e in contexto.Empleados on h.idEmpleado equals e.idEmpleado
                               join t in contexto.TiposEventoHistorial on h.idTipoEvento equals t.idTipoEvento
                               join est in contexto.Estado on h.idEstado equals est.idEstado
                               join u in contexto.Users on h.idUsuarioModificacion equals u.Id into usuarios
                               from u in usuarios.DefaultIfEmpty()
                               where h.idEmpleado == idEmpleado
                                     && h.idEstado == 1
                                     && (idTipoEvento == null || h.idTipoEvento == idTipoEvento)
                                     && (string.IsNullOrEmpty(categoriaEvento) || t.categoriaEvento == categoriaEvento)
                                     && h.fechaEvento >= fechaInicio.Value
                                     && h.fechaEvento <= fechaFin.Value
                               orderby h.fechaEvento descending
                               select new HistorialEmpleadoDto
                               {
                                   idHistorial = h.idHistorial,
                                   idEmpleado = h.idEmpleado,
                                   nombreEmpleado = e.nombre + " " + e.primerApellido,
                                   idTipoEvento = h.idTipoEvento,
                                   nombreEvento = t.nombreEvento,
                                   descripcionTipoEvento = t.descripcionEvento,
                                   categoriaEvento = t.categoriaEvento,
                                   iconoEvento = t.iconoEvento,
                                   colorEvento = t.colorEvento,
                                   fechaEvento = h.fechaEvento,
                                   descripcionEvento = h.descripcionEvento,
                                   detallesEvento = h.detallesEvento,
                                   valorAnterior = h.valorAnterior,
                                   valorNuevo = h.valorNuevo,
                                   idUsuarioModificacion = h.idUsuarioModificacion,
                                   nombreUsuarioModificacion = u != null ? u.UserName : "Sistema",
                                   ipModificacion = h.ipModificacion,
                                   idEstado = h.idEstado,
                                   nombreEstado = est.nombreEstado,
                                   fechaCreacion = h.fechaCreacion
                               };

                    return query.Take(top).ToList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en ObtenerHistorialEmpleado: {ex.Message}");
                return new List<HistorialEmpleadoDto>();
            }
        }

        public List<HistorialEmpleadoDto> ObtenerHistorialPorCategoria(int idEmpleado, string categoriaEvento, int top = 100)
        {
            return ObtenerHistorialEmpleado(idEmpleado, null, null, null, categoriaEvento, top);
        }

        public List<HistorialEmpleadoDto> ObtenerHistorialPorTipoEvento(int idEmpleado, int idTipoEvento, int top = 100)
        {
            return ObtenerHistorialEmpleado(idEmpleado, idTipoEvento, null, null, null, top);
        }

        public List<HistorialEmpleadoDto> ObtenerHistorialPorFecha(int idEmpleado, DateTime fechaInicio, DateTime fechaFin, int top = 100)
        {
            return ObtenerHistorialEmpleado(idEmpleado, null, fechaInicio, fechaFin, null, top);
        }

        public int ObtenerTotalEventos(int idEmpleado)
        {
            try
            {
                using (var contexto = new Contexto())
                {
                    return contexto.HistorialEmpleado
                        .Where(h => h.idEmpleado == idEmpleado && h.idEstado == 1)
                        .Count();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en ObtenerTotalEventos: {ex.Message}");
                return 0;
            }
        }

        public List<string> ObtenerCategoriasDisponibles()
        {
            try
            {
                using (var contexto = new Contexto())
                {
                    return contexto.TiposEventoHistorial
                        .Where(t => t.idEstado == 1)
                        .Select(t => t.categoriaEvento)
                        .Distinct()
                        .OrderBy(c => c)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en ObtenerCategoriasDisponibles: {ex.Message}");
                return new List<string>();
            }
        }
    }
}
