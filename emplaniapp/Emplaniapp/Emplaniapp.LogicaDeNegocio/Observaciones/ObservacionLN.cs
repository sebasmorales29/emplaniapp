using Emplaniapp.Abstracciones.InterfacesAD;
using Emplaniapp.Abstracciones.InterfacesParaUI;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos;
using System.Collections.Generic;
using System.Linq;

namespace Emplaniapp.LogicaDeNegocio
{
    public class ObservacionLN : IObservacionLN
    {
        private readonly IObservacionAD _observacionAD;

        public ObservacionLN()
        {
            _observacionAD = new ObservacionAD();
        }

        public List<ObservacionDto> ObtenerObservacionesPorEmpleado(int idEmpleado)
        {
            var observaciones = _observacionAD.ObtenerObservacionesPorEmpleado(idEmpleado);
            return observaciones.Select(o => new ObservacionDto
            {
                IdObservacion = o.IdObservacion,
                IdEmpleado = o.IdEmpleado,
                Titulo = o.Titulo,
                Descripcion = o.Descripcion,
                IdUsuarioCreo = o.IdUsuarioCreo,
                FechaCreacion = o.FechaCreacion,
                IdUsuarioEdito = o.IdUsuarioEdito,
                FechaEdicion = o.FechaEdicion
            }).ToList();
        }

        public ObservacionDto ObtenerObservacionPorId(int idObservacion)
        {
            var o = _observacionAD.ObtenerObservacionPorId(idObservacion);
            if (o == null) return null;

            return new ObservacionDto
            {
                IdObservacion = o.IdObservacion,
                IdEmpleado = o.IdEmpleado,
                Titulo = o.Titulo,
                Descripcion = o.Descripcion,
                IdUsuarioCreo = o.IdUsuarioCreo,
                FechaCreacion = o.FechaCreacion,
                IdUsuarioEdito = o.IdUsuarioEdito,
                FechaEdicion = o.FechaEdicion
            };
        }

        public bool GuardarObservacion(ObservacionDto observacionDto)
        {
            // Aquí irían validaciones de negocio
            if (string.IsNullOrWhiteSpace(observacionDto.Titulo) || string.IsNullOrWhiteSpace(observacionDto.Descripcion))
            {
                return false;
            }

            var observacion = new Abstracciones.Entidades.Observacion
            {
                IdObservacion = observacionDto.IdObservacion,
                IdEmpleado = observacionDto.IdEmpleado,
                Titulo = observacionDto.Titulo,
                Descripcion = observacionDto.Descripcion,
                IdUsuarioCreo = observacionDto.IdUsuarioCreo,
                FechaCreacion = observacionDto.FechaCreacion,
                IdUsuarioEdito = observacionDto.IdUsuarioEdito,
                FechaEdicion = observacionDto.FechaEdicion
            };

            return _observacionAD.GuardarObservacion(observacion);
        }

        public bool ActualizarObservacion(ObservacionDto observacionDto)
        {
            if (string.IsNullOrWhiteSpace(observacionDto.Titulo) || string.IsNullOrWhiteSpace(observacionDto.Descripcion))
            {
                return false;
            }

            return _observacionAD.ActualizarObservacion(observacionDto);
        }

        public bool EliminarObservacion(int idObservacion)
        {
            return _observacionAD.EliminarObservacion(idObservacion);
        }
    }
} 