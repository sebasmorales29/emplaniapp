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
    public class RegistrarEventoHistorialAD : IRegistrarEventoHistorial
    {
        public bool RegistrarEvento(int idEmpleado, string nombreEvento, string descripcionEvento, string detallesEvento = null, string valorAnterior = null, string valorNuevo = null, string idUsuarioModificacion = null, string ipModificacion = null)
        {
            try
            {
                using (var contexto = new Contexto())
                {
                    // Obtener el ID del tipo de evento
                    var tipoEvento = contexto.TiposEventoHistorial
                        .FirstOrDefault(t => t.nombreEvento == nombreEvento && t.idEstado == 1);

                    int idTipoEvento;
                    if (tipoEvento != null)
                    {
                        idTipoEvento = tipoEvento.idTipoEvento;
                    }
                    else
                    {
                        // Si no existe, crear uno genérico
                        var nuevoTipo = new TiposEventoHistorial
                        {
                            nombreEvento = nombreEvento,
                            descripcionEvento = "Evento personalizado",
                            categoriaEvento = "Sistema",
                            iconoEvento = "info-circle",
                            colorEvento = "secondary",
                            idEstado = 1,
                            fechaCreacion = DateTime.Now
                        };

                        contexto.TiposEventoHistorial.Add(nuevoTipo);
                        contexto.SaveChanges();
                        idTipoEvento = nuevoTipo.idTipoEvento;
                    }

                    // Crear el evento en el historial
                    var evento = new HistorialEmpleado
                    {
                        idEmpleado = idEmpleado,
                        idTipoEvento = idTipoEvento,
                        fechaEvento = DateTime.Now,
                        descripcionEvento = descripcionEvento,
                        detallesEvento = detallesEvento,
                        valorAnterior = valorAnterior,
                        valorNuevo = valorNuevo,
                        idUsuarioModificacion = idUsuarioModificacion,
                        ipModificacion = ipModificacion,
                        idEstado = 1,
                        fechaCreacion = DateTime.Now
                    };

                    contexto.HistorialEmpleado.Add(evento);
                    contexto.SaveChanges();

                    System.Diagnostics.Debug.WriteLine($"✅ Evento '{nombreEvento}' registrado exitosamente para empleado {idEmpleado}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error al registrar evento '{nombreEvento}': {ex.Message}");
                return false;
            }
        }

        public bool RegistrarEventoPersonalizado(int idEmpleado, string nombreEvento, string descripcionEvento, string categoriaEvento = "Sistema", string iconoEvento = "info-circle", string colorEvento = "secondary", string detallesEvento = null, string valorAnterior = null, string valorNuevo = null, string idUsuarioModificacion = null, string ipModificacion = null)
        {
            try
            {
                using (var contexto = new Contexto())
                {
                    // Crear tipo de evento personalizado
                    var tipoEvento = new TiposEventoHistorial
                    {
                        nombreEvento = nombreEvento,
                        descripcionEvento = descripcionEvento,
                        categoriaEvento = categoriaEvento,
                        iconoEvento = iconoEvento,
                        colorEvento = colorEvento,
                        idEstado = 1,
                        fechaCreacion = DateTime.Now
                    };

                    contexto.TiposEventoHistorial.Add(tipoEvento);
                    contexto.SaveChanges();

                    // Registrar el evento
                    return RegistrarEvento(idEmpleado, nombreEvento, descripcionEvento, detallesEvento, valorAnterior, valorNuevo, idUsuarioModificacion, ipModificacion);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error al registrar evento personalizado '{nombreEvento}': {ex.Message}");
                return false;
            }
        }

        public bool RegistrarCambioEmpleado(int idEmpleado, string nombreEvento, string descripcionEvento, string valorAnterior, string valorNuevo, string idUsuarioModificacion = null, string ipModificacion = null)
        {
            return RegistrarEvento(idEmpleado, nombreEvento, descripcionEvento, null, valorAnterior, valorNuevo, idUsuarioModificacion, ipModificacion);
        }

        public bool RegistrarCreacionEmpleado(int idEmpleado, string idUsuarioModificacion = null, string ipModificacion = null)
        {
            return RegistrarEvento(idEmpleado, "Creación de Empleado", "Se creó un nuevo empleado en el sistema", null, null, null, idUsuarioModificacion, ipModificacion);
        }

        public bool RegistrarLiquidacion(int idEmpleado, string motivo, decimal costoTotal, string idUsuarioModificacion = null, string ipModificacion = null)
        {
            var detalles = $"Motivo: {motivo}, Costo Total: ₡{costoTotal:N2}";
            return RegistrarEvento(idEmpleado, "Liquidación", "Se procesó la liquidación del empleado", detalles, null, null, idUsuarioModificacion, ipModificacion);
        }

        public bool RegistrarRemuneracion(int idEmpleado, decimal monto, string tipoRemuneracion, string idUsuarioModificacion = null, string ipModificacion = null)
        {
            var detalles = $"Tipo: {tipoRemuneracion}, Monto: ₡{monto:N2}";
            return RegistrarEvento(idEmpleado, "Generación de Remuneración", "Se generó una remuneración para el empleado", detalles, null, null, idUsuarioModificacion, ipModificacion);
        }

        public bool RegistrarRetencion(int idEmpleado, decimal monto, string tipoRetencion, string idUsuarioModificacion = null, string ipModificacion = null)
        {
            var detalles = $"Tipo: {tipoRetencion}, Monto: ₡{monto:N2}";
            return RegistrarEvento(idEmpleado, "Aplicación de Retención", "Se aplicó una retención al empleado", detalles, null, null, idUsuarioModificacion, ipModificacion);
        }
    }
}
