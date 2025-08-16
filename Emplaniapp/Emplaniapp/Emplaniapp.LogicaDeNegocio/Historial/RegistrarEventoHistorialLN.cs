using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Historial;
using Emplaniapp.Abstracciones.InterfacesParaUI.Historial;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Historial;
using System.Web;
using System.Net;

namespace Emplaniapp.LogicaDeNegocio.Historial
{
    public class RegistrarEventoHistorialLN : IRegistrarEventoHistorialLN
    {
        private IRegistrarEventoHistorial _registrarEventoHistorialAD;

        public RegistrarEventoHistorialLN()
        {
            _registrarEventoHistorialAD = new RegistrarEventoHistorialAD();
        }

        public bool RegistrarEvento(int idEmpleado, string nombreEvento, string descripcionEvento, string detallesEvento = null, string valorAnterior = null, string valorNuevo = null, string idUsuarioModificacion = null, string ipModificacion = null)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"📝 Registrando evento '{nombreEvento}' para empleado {idEmpleado}");

                // Validaciones
                if (!ValidarDatosEvento(idEmpleado, nombreEvento, descripcionEvento))
                {
                    return false;
                }

                // Obtener IP si no se proporciona
                if (string.IsNullOrEmpty(ipModificacion))
                {
                    ipModificacion = ObtenerIPUsuario();
                }

                // Obtener usuario si no se proporciona
                if (string.IsNullOrEmpty(idUsuarioModificacion))
                {
                    idUsuarioModificacion = ObtenerUsuarioActual();
                }

                var resultado = _registrarEventoHistorialAD.RegistrarEvento(idEmpleado, nombreEvento, descripcionEvento, detallesEvento, valorAnterior, valorNuevo, idUsuarioModificacion, ipModificacion);
                
                if (resultado)
                {
                    System.Diagnostics.Debug.WriteLine($"✅ Evento '{nombreEvento}' registrado exitosamente");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Error al registrar evento '{nombreEvento}'");
                }

                return resultado;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en RegistrarEvento LN: {ex.Message}");
                return false;
            }
        }

        public bool RegistrarEventoPersonalizado(int idEmpleado, string nombreEvento, string descripcionEvento, string categoriaEvento = "Sistema", string iconoEvento = "info-circle", string colorEvento = "secondary", string detallesEvento = null, string valorAnterior = null, string valorNuevo = null, string idUsuarioModificacion = null, string ipModificacion = null)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"📝 Registrando evento personalizado '{nombreEvento}' para empleado {idEmpleado}");

                // Validaciones
                if (!ValidarDatosEvento(idEmpleado, nombreEvento, descripcionEvento))
                {
                    return false;
                }

                // Validar categoría
                if (string.IsNullOrWhiteSpace(categoriaEvento))
                {
                    categoriaEvento = "Sistema";
                }

                // Obtener IP si no se proporciona
                if (string.IsNullOrEmpty(ipModificacion))
                {
                    ipModificacion = ObtenerIPUsuario();
                }

                // Obtener usuario si no se proporciona
                if (string.IsNullOrEmpty(idUsuarioModificacion))
                {
                    idUsuarioModificacion = ObtenerUsuarioActual();
                }

                var resultado = _registrarEventoHistorialAD.RegistrarEventoPersonalizado(idEmpleado, nombreEvento, descripcionEvento, categoriaEvento, iconoEvento, colorEvento, detallesEvento, valorAnterior, valorNuevo, idUsuarioModificacion, ipModificacion);
                
                if (resultado)
                {
                    System.Diagnostics.Debug.WriteLine($"✅ Evento personalizado '{nombreEvento}' registrado exitosamente");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Error al registrar evento personalizado '{nombreEvento}'");
                }

                return resultado;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en RegistrarEventoPersonalizado LN: {ex.Message}");
                return false;
            }
        }

        public bool RegistrarCambioEmpleado(int idEmpleado, string nombreEvento, string descripcionEvento, string valorAnterior, string valorNuevo, string idUsuarioModificacion = null, string ipModificacion = null)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"📝 Registrando cambio de empleado: {nombreEvento}");

                // Validaciones adicionales para cambios
                if (string.IsNullOrWhiteSpace(valorAnterior) && string.IsNullOrWhiteSpace(valorNuevo))
                {
                    System.Diagnostics.Debug.WriteLine("⚠️ Advertencia: No hay valores para comparar en el cambio");
                }

                return RegistrarEvento(idEmpleado, nombreEvento, descripcionEvento, null, valorAnterior, valorNuevo, idUsuarioModificacion, ipModificacion);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en RegistrarCambioEmpleado LN: {ex.Message}");
                return false;
            }
        }

        public bool RegistrarCreacionEmpleado(int idEmpleado, string idUsuarioModificacion = null, string ipModificacion = null)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"📝 Registrando creación de empleado {idEmpleado}");

                return RegistrarEvento(idEmpleado, "Creación de Empleado", "Se creó un nuevo empleado en el sistema", null, null, null, idUsuarioModificacion, ipModificacion);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en RegistrarCreacionEmpleado LN: {ex.Message}");
                return false;
            }
        }

        public bool RegistrarLiquidacion(int idEmpleado, string motivo, decimal costoTotal, string idUsuarioModificacion = null, string ipModificacion = null)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"📝 Registrando liquidación para empleado {idEmpleado}");

                if (string.IsNullOrWhiteSpace(motivo))
                {
                    System.Diagnostics.Debug.WriteLine("⚠️ Advertencia: Motivo de liquidación no especificado");
                    motivo = "No especificado";
                }

                if (costoTotal < 0)
                {
                    System.Diagnostics.Debug.WriteLine("⚠️ Advertencia: Costo total negativo, ajustando a 0");
                    costoTotal = 0;
                }

                return RegistrarEvento(idEmpleado, "Liquidación", "Se procesó la liquidación del empleado", $"Motivo: {motivo}, Costo Total: ₡{costoTotal:N2}", null, null, idUsuarioModificacion, ipModificacion);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en RegistrarLiquidacion LN: {ex.Message}");
                return false;
            }
        }

        public bool RegistrarRemuneracion(int idEmpleado, decimal monto, string tipoRemuneracion, string idUsuarioModificacion = null, string ipModificacion = null)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"📝 Registrando remuneración para empleado {idEmpleado}");

                if (monto <= 0)
                {
                    System.Diagnostics.Debug.WriteLine("❌ Error: Monto de remuneración debe ser mayor a 0");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(tipoRemuneracion))
                {
                    System.Diagnostics.Debug.WriteLine("⚠️ Advertencia: Tipo de remuneración no especificado");
                    tipoRemuneracion = "No especificado";
                }

                return RegistrarEvento(idEmpleado, "Generación de Remuneración", "Se generó una remuneración para el empleado", $"Tipo: {tipoRemuneracion}, Monto: ₡{monto:N2}", null, null, idUsuarioModificacion, ipModificacion);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en RegistrarRemuneracion LN: {ex.Message}");
                return false;
            }
        }

        public bool RegistrarRetencion(int idEmpleado, decimal monto, string tipoRetencion, string idUsuarioModificacion = null, string ipModificacion = null)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"📝 Registrando retención para empleado {idEmpleado}");

                if (monto <= 0)
                {
                    System.Diagnostics.Debug.WriteLine("❌ Error: Monto de retención debe ser mayor a 0");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(tipoRetencion))
                {
                    System.Diagnostics.Debug.WriteLine("⚠️ Advertencia: Tipo de retención no especificado");
                    tipoRetencion = "No especificado";
                }

                return RegistrarEvento(idEmpleado, "Aplicación de Retención", "Se aplicó una retención al empleado", $"Tipo: {tipoRetencion}, Monto: ₡{monto:N2}", null, null, idUsuarioModificacion, ipModificacion);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en RegistrarRetencion LN: {ex.Message}");
                return false;
            }
        }

        public bool ValidarDatosEvento(int idEmpleado, string nombreEvento, string descripcionEvento)
        {
            try
            {
                if (idEmpleado <= 0)
                {
                    System.Diagnostics.Debug.WriteLine("❌ Error: ID de empleado inválido");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(nombreEvento))
                {
                    System.Diagnostics.Debug.WriteLine("❌ Error: Nombre del evento es obligatorio");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(descripcionEvento))
                {
                    System.Diagnostics.Debug.WriteLine("❌ Error: Descripción del evento es obligatoria");
                    return false;
                }

                if (nombreEvento.Length > 100)
                {
                    System.Diagnostics.Debug.WriteLine("❌ Error: Nombre del evento excede 100 caracteres");
                    return false;
                }

                if (descripcionEvento.Length > 1000)
                {
                    System.Diagnostics.Debug.WriteLine("❌ Error: Descripción del evento excede 1000 caracteres");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en ValidarDatosEvento: {ex.Message}");
                return false;
            }
        }

        public string ObtenerIPUsuario()
        {
            try
            {
                // En la capa de lógica de negocio, no podemos acceder a HttpContext
                // Por lo tanto, devolvemos un valor por defecto
                // La IP real debe ser obtenida en la capa de presentación y pasada como parámetro
                System.Diagnostics.Debug.WriteLine("🌐 ObtenerIPUsuario llamado desde LN - devolviendo IP por defecto");
                return "127.0.0.1"; // IP por defecto
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en ObtenerIPUsuario: {ex.Message}");
                return "127.0.0.1"; // Fallback
            }
        }

        private string ObtenerUsuarioActual()
        {
            try
            {
                // En la capa de lógica de negocio, no podemos acceder a HttpContext
                // Por lo tanto, devolvemos un valor por defecto
                // El usuario real debe ser obtenido en la capa de presentación y pasado como parámetro
                System.Diagnostics.Debug.WriteLine("👤 ObtenerUsuarioActual llamado desde LN - devolviendo usuario por defecto");
                return "Sistema"; // Usuario por defecto
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en ObtenerUsuarioActual: {ex.Message}");
                return "Sistema"; // Fallback
            }
        }
    }
}
