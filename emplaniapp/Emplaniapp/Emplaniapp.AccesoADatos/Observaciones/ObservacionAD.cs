using Emplaniapp.Abstracciones.Entidades;
using Emplaniapp.Abstracciones.InterfacesAD;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Emplaniapp.AccesoADatos
{
    public class ObservacionAD : IObservacionAD
    {
        private readonly Contexto _contexto;

        public ObservacionAD()
        {
            _contexto = new Contexto();
        }

        public List<Observacion> ObtenerObservacionesPorEmpleado(int idEmpleado)
        {
            return _contexto.Observaciones
                            .Where(o => o.IdEmpleado == idEmpleado)
                            .OrderByDescending(o => o.FechaCreacion)
                            .ToList();
        }

        public Observacion ObtenerObservacionPorId(int idObservacion)
        {
            return _contexto.Observaciones.Find(idObservacion);
        }

        public bool GuardarObservacion(Observacion observacion)
        {
            if (observacion.IdObservacion > 0)
            {
                // Editar
                _contexto.Entry(observacion).State = EntityState.Modified;
            }
            else
            {
                // Crear
                _contexto.Observaciones.Add(observacion);
            }
            return _contexto.SaveChanges() > 0;
        }

        public bool ActualizarObservacion(Abstracciones.ModelosParaUI.ObservacionDto observacionDto)
        {
            var observacionEnDB = _contexto.Observaciones.Find(observacionDto.IdObservacion);
            if (observacionEnDB == null)
            {
                return false;
            }

            // Actualizar solo los campos necesarios
            observacionEnDB.Titulo = observacionDto.Titulo;
            observacionEnDB.Descripcion = observacionDto.Descripcion;
            observacionEnDB.IdUsuarioEdito = observacionDto.IdUsuarioEdito;
            observacionEnDB.FechaEdicion = observacionDto.FechaEdicion;

            return _contexto.SaveChanges() > 0;
        }

        public bool EliminarObservacion(int idObservacion)
        {
            var observacion = _contexto.Observaciones.Find(idObservacion);
            if (observacion == null)
            {
                return false;
            }

            _contexto.Observaciones.Remove(observacion);
            return _contexto.SaveChanges() > 0;
        }
    }
} 