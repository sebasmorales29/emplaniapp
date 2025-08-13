using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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
                    "EXEC sp_GenerarRemuneracionesQuincenales @FechaProceso", fechaParam).ToList();

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar remuneraciones quincenales: " + ex.Message);
            }
        }

        public async Task<int> AgregarRemuneracionManual(RemuneracionDto remuneracionDto)
        {
            try
            {
                var empleado = await _contexto.Empleados
                    .Where(e => e.idEmpleado == remuneracionDto.idEmpleado)
                    .Select(e => new { e.salarioPorHoraExtra, e.salarioDiario })
                    .FirstOrDefaultAsync();

                if (empleado == null)
                    throw new Exception("Empleado no encontrado.");

                decimal salarioDiario = empleado.salarioDiario;

                switch (remuneracionDto.idTipoRemuneracion)
                {
                    case 1: // Horas Extra
                        remuneracionDto.pagoQuincenal = remuneracionDto.horas.HasValue
                            ? remuneracionDto.horas.Value * empleado.salarioPorHoraExtra
                            : 0;
                        break;

                    case 2: // Día Feriado
                        remuneracionDto.pagoQuincenal = remuneracionDto.TrabajoEnDia
                            ? salarioDiario * 2
                            : salarioDiario;
                        break;

                    case 3: // Incapacidad (primeros 3 días solamente, mitad de salario)
                        remuneracionDto.pagoQuincenal = (salarioDiario / 2) * 3;
                        break;

                    case 4: // Maternidad (mitad de salario por quincena)
                        remuneracionDto.pagoQuincenal = salarioDiario * 15 / 2;
                        break;

                    case 5: // Vacaciones
                        remuneracionDto.pagoQuincenal = remuneracionDto.TrabajoEnDia
                            ? salarioDiario
                            : 0;
                        break;

                    case 6: // Pago Quincenal
                        remuneracionDto.pagoQuincenal = remuneracionDto.diasTrabajados.HasValue
                            ? remuneracionDto.diasTrabajados.Value * salarioDiario
                            : 0;
                        break;

                    default:
                        remuneracionDto.pagoQuincenal = 0;
                        break;
                }


                Remuneracion laRemuneracionAGuardar = ConvertirDtoAEntidad(remuneracionDto);
                _contexto.Remuneracion.Add(laRemuneracionAGuardar);
                int cantidadDatosAgregados = await _contexto.SaveChangesAsync();
                return cantidadDatosAgregados;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar la remuneración manual: " + ex.Message);
            }
        }

        private Remuneracion ConvertirDtoAEntidad(RemuneracionDto dto)
        {
            return new Remuneracion
            {
                idEmpleado = dto.idEmpleado,
                idTipoRemuneracion = dto.idTipoRemuneracion,
                fechaRemuneracion = dto.fechaRemuneracion,
                pagoQuincenal = dto.pagoQuincenal,
                diasTrabajados = dto.diasTrabajados,
                horas = dto.horas,
                comision = dto.comision,
                idEstado = dto.idEstado
            };
        }

    }
}
