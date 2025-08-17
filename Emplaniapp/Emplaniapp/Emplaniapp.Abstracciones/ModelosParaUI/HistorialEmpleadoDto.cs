using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Emplaniapp.Abstracciones.ModelosParaUI
{
    public class HistorialEmpleadoDto
    {
        [DisplayName("ID Historial")]
        public int idHistorial { get; set; }

        [DisplayName("ID Empleado")]
        public int idEmpleado { get; set; }

        [DisplayName("Nombre del Empleado")]
        public string nombreEmpleado { get; set; }

        [DisplayName("ID Tipo Evento")]
        public int idTipoEvento { get; set; }

        [DisplayName("Nombre del Evento")]
        public string nombreEvento { get; set; }

        [DisplayName("Descripción del Tipo")]
        public string descripcionTipoEvento { get; set; }

        [DisplayName("Categoría del Evento")]
        public string categoriaEvento { get; set; }

        [DisplayName("Icono del Evento")]
        public string iconoEvento { get; set; }

        [DisplayName("Color del Evento")]
        public string colorEvento { get; set; }

        [DisplayName("Fecha del Evento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime fechaEvento { get; set; }

        [DisplayName("Descripción del Evento")]
        public string descripcionEvento { get; set; }

        [DisplayName("Detalles del Evento")]
        public string detallesEvento { get; set; }

        [DisplayName("Valor Anterior")]
        public string valorAnterior { get; set; }

        [DisplayName("Valor Nuevo")]
        public string valorNuevo { get; set; }

        [DisplayName("ID Usuario Modificación")]
        public string idUsuarioModificacion { get; set; }

        [DisplayName("Usuario que Modificó")]
        public string nombreUsuarioModificacion { get; set; }

        [DisplayName("IP de Modificación")]
        public string ipModificacion { get; set; }

        [DisplayName("ID Estado")]
        public int idEstado { get; set; }

        [DisplayName("Estado")]
        public string nombreEstado { get; set; }

        [DisplayName("Fecha de Creación")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime fechaCreacion { get; set; }

        // Propiedades para filtros
        [DisplayName("Fecha Inicio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? fechaInicio { get; set; }

        [DisplayName("Fecha Fin")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? fechaFin { get; set; }

        [DisplayName("Cantidad de Registros")]
        public int top { get; set; } = 100;

        // Propiedades para la UI
        [DisplayName("Fecha Formateada")]
        public string fechaEventoFormateada 
        { 
            get 
            { 
                return fechaEvento.ToString("dd/MM/yyyy HH:mm"); 
            } 
        }

        [DisplayName("Tiempo Transcurrido")]
        public string tiempoTranscurrido
        {
            get
            {
                var tiempo = DateTime.Now - fechaEvento;
                if (tiempo.TotalDays >= 1)
                    return $"{(int)tiempo.TotalDays} día(s)";
                else if (tiempo.TotalHours >= 1)
                    return $"{(int)tiempo.TotalHours} hora(s)";
                else if (tiempo.TotalMinutes >= 1)
                    return $"{(int)tiempo.TotalMinutes} minuto(s)";
                else
                    return "Hace un momento";
            }
        }

        [DisplayName("Clase CSS del Icono")]
        public string claseIcono
        {
            get
            {
                return $"fas fa-{iconoEvento}";
            }
        }

        [DisplayName("Clase CSS del Color")]
        public string claseColor
        {
            get
            {
                return $"text-{colorEvento}";
            }
        }
    }
}
