using System;
using System.Collections.Generic;
using System.Linq;
using Emplaniapp.Abstracciones.InterfacesAD.Remuneraciones.CrearRemuneracion;
using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.AccesoADatos.Remuneraciones
{
    public class CrearRemuneracionesAD : ICrearRemuneracionesAD
    {
        private Contexto _contexto;


        public CrearRemuneracionesAD()
        {
            _contexto = new Contexto();
        }

        public List<RemuneracionDto> GenerarRemuneracionesQuincenales(DateTime? fechaProceso = null)
        {
            try
            {
                var fechaParam = fechaProceso.HasValue
                    ? new System.Data.SqlClient.SqlParameter("@FechaProceso", fechaProceso.Value)
                    : new System.Data.SqlClient.SqlParameter("@FechaProceso", DBNull.Value);

                var resultado = _contexto.Database.SqlQuery<RemuneracionDto>(
                    "EXEC sp_GenerarRemuneracionesQuincenales @FechaProceso",
                    fechaParam).ToList();

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar remuneraciones quincenales: " + ex.Message);
            }
        }

        public void AgregarRemuneracionManual(RemuneracionDto remuneracionDto, int idEmpleado)
        {
            try
            {
                // Buscar el empleado por cédula o identificación
                var empleado = _contexto.Empleados
                    .FirstOrDefault(e => e.idEmpleado == idEmpleado);


                var remuneracion = new Remuneracion
                {
                    idEmpleado = empleado.idEmpleado, // Obtenido de la búsqueda
                    idTipoRemuneracion = remuneracionDto.idTipoRemuneracion,
                    fechaRemuneracion = remuneracionDto.fechaRemuneracion,
                    pagoQuincenal = remuneracionDto.pagoQuincenal,
                    horasTrabajadas = remuneracionDto.horasTrabajadas,
                    horasExtras = remuneracionDto.horasExtras,
                    comision = remuneracionDto.comision,
                    horasFeriados = remuneracionDto.horasFeriados,
                    horasVacaciones = remuneracionDto.horasVacaciones,
                    horasLicencias = remuneracionDto.horasLicencias,
                    idEstado = remuneracionDto.idEstado
                };

                _contexto.Remuneracion.Add(remuneracion);
                _contexto.SaveChanges();

                // Actualizar el DTO con los datos generados
                remuneracionDto.idRemuneracion = remuneracion.idRemuneracion;
                remuneracionDto.idEmpleado = empleado.idEmpleado;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar remuneración manual: " + ex.Message);
            }
        }

    }
}
