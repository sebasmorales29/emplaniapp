using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Historial;
using Emplaniapp.Abstracciones.InterfacesParaUI.Historial;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Historial;

namespace Emplaniapp.LogicaDeNegocio.Historial
{
    public class ConsultarHistorialLN : IConsultarHistorialLN
    {
        private IConsultarHistorial _consultarHistorialAD;

        public ConsultarHistorialLN()
        {
            _consultarHistorialAD = new ConsultarHistorialAD();
        }

        public List<HistorialEmpleadoDto> ObtenerHistorialEmpleado(int idEmpleado, int? idTipoEvento = null, DateTime? fechaInicio = null, DateTime? fechaFin = null, string categoriaEvento = null, int top = 100)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"🔍 Consultando historial para empleado {idEmpleado}");
                
                // Validaciones básicas
                if (idEmpleado <= 0)
                {
                    System.Diagnostics.Debug.WriteLine("❌ Error: ID de empleado inválido");
                    return new List<HistorialEmpleadoDto>();
                }

                if (top <= 0 || top > 1000)
                {
                    System.Diagnostics.Debug.WriteLine("⚠️ Ajustando top a 100 (máximo permitido)");
                    top = 100;
                }

                // Validar fechas
                if (fechaInicio.HasValue && fechaFin.HasValue && fechaInicio > fechaFin)
                {
                    System.Diagnostics.Debug.WriteLine("⚠️ Fecha inicio mayor a fecha fin, intercambiando...");
                    var temp = fechaInicio;
                    fechaInicio = fechaFin;
                    fechaFin = temp;
                }

                var historial = _consultarHistorialAD.ObtenerHistorialEmpleado(idEmpleado, idTipoEvento, fechaInicio, fechaFin, categoriaEvento, top);
                
                System.Diagnostics.Debug.WriteLine($"✅ Historial obtenido: {historial.Count} eventos");
                return historial;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en ObtenerHistorialEmpleado LN: {ex.Message}");
                return new List<HistorialEmpleadoDto>();
            }
        }

        public List<HistorialEmpleadoDto> ObtenerHistorialPorCategoria(int idEmpleado, string categoriaEvento, int top = 100)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(categoriaEvento))
                {
                    System.Diagnostics.Debug.WriteLine("❌ Error: Categoría de evento no especificada");
                    return new List<HistorialEmpleadoDto>();
                }

                System.Diagnostics.Debug.WriteLine($"🔍 Consultando historial por categoría: {categoriaEvento}");
                return _consultarHistorialAD.ObtenerHistorialPorCategoria(idEmpleado, categoriaEvento, top);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en ObtenerHistorialPorCategoria LN: {ex.Message}");
                return new List<HistorialEmpleadoDto>();
            }
        }

        public List<HistorialEmpleadoDto> ObtenerHistorialPorTipoEvento(int idEmpleado, int idTipoEvento, int top = 100)
        {
            try
            {
                if (idTipoEvento <= 0)
                {
                    System.Diagnostics.Debug.WriteLine("❌ Error: ID de tipo de evento inválido");
                    return new List<HistorialEmpleadoDto>();
                }

                System.Diagnostics.Debug.WriteLine($"🔍 Consultando historial por tipo de evento: {idTipoEvento}");
                return _consultarHistorialAD.ObtenerHistorialPorTipoEvento(idEmpleado, idTipoEvento, top);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en ObtenerHistorialPorTipoEvento LN: {ex.Message}");
                return new List<HistorialEmpleadoDto>();
            }
        }

        public List<HistorialEmpleadoDto> ObtenerHistorialPorFecha(int idEmpleado, DateTime fechaInicio, DateTime fechaFin, int top = 100)
        {
            try
            {
                if (fechaInicio > fechaFin)
                {
                    System.Diagnostics.Debug.WriteLine("❌ Error: Fecha inicio mayor a fecha fin");
                    return new List<HistorialEmpleadoDto>();
                }

                System.Diagnostics.Debug.WriteLine($"🔍 Consultando historial por fecha: {fechaInicio:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy}");
                return _consultarHistorialAD.ObtenerHistorialPorFecha(idEmpleado, fechaInicio, fechaFin, top);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en ObtenerHistorialPorFecha LN: {ex.Message}");
                return new List<HistorialEmpleadoDto>();
            }
        }

        public int ObtenerTotalEventos(int idEmpleado)
        {
            try
            {
                if (idEmpleado <= 0)
                {
                    System.Diagnostics.Debug.WriteLine("❌ Error: ID de empleado inválido");
                    return 0;
                }

                var total = _consultarHistorialAD.ObtenerTotalEventos(idEmpleado);
                System.Diagnostics.Debug.WriteLine($"📊 Total de eventos para empleado {idEmpleado}: {total}");
                return total;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en ObtenerTotalEventos LN: {ex.Message}");
                return 0;
            }
        }

        public List<string> ObtenerCategoriasDisponibles()
        {
            try
            {
                var categorias = _consultarHistorialAD.ObtenerCategoriasDisponibles();
                System.Diagnostics.Debug.WriteLine($"📋 Categorías disponibles: {string.Join(", ", categorias)}");
                return categorias;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en ObtenerCategoriasDisponibles LN: {ex.Message}");
                return new List<string>();
            }
        }

        public List<HistorialEmpleadoDto> ObtenerHistorialReciente(int idEmpleado, int cantidad = 10)
        {
            try
            {
                if (cantidad <= 0 || cantidad > 100)
                {
                    cantidad = 10;
                }

                System.Diagnostics.Debug.WriteLine($"🔍 Consultando historial reciente: {cantidad} eventos");
                return ObtenerHistorialEmpleado(idEmpleado, null, null, null, null, cantidad);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en ObtenerHistorialReciente LN: {ex.Message}");
                return new List<HistorialEmpleadoDto>();
            }
        }

        public List<HistorialEmpleadoDto> ObtenerHistorialPorPeriodo(int idEmpleado, string periodo = "mes")
        {
            try
            {
                DateTime fechaInicio;
                DateTime fechaFin = DateTime.Now;

                switch (periodo.ToLower())
                {
                    case "dia":
                        fechaInicio = fechaFin.Date;
                        break;
                    case "semana":
                        fechaInicio = fechaFin.AddDays(-7);
                        break;
                    case "mes":
                        fechaInicio = fechaFin.AddMonths(-1);
                        break;
                    case "anio":
                        fechaInicio = fechaFin.AddYears(-1);
                        break;
                    default:
                        fechaInicio = fechaFin.AddMonths(-1);
                        break;
                }

                System.Diagnostics.Debug.WriteLine($"🔍 Consultando historial por período: {periodo} ({fechaInicio:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy})");
                return ObtenerHistorialPorFecha(idEmpleado, fechaInicio, fechaFin);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en ObtenerHistorialPorPeriodo LN: {ex.Message}");
                return new List<HistorialEmpleadoDto>();
            }
        }
    }
}
