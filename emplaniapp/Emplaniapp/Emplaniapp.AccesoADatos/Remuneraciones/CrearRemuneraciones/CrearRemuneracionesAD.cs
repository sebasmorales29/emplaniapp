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
                // Traer empleado y nombre de cargo desde tabla relacionada
                var empleadoConCargo = await (
                    from e in _contexto.Empleados
                    join c in _contexto.Cargos on e.idCargo equals c.idCargo
                    where e.idEmpleado == remuneracionDto.idEmpleado
                    select new
                    {
                        e.salarioPorHoraExtra,
                        e.salarioDiario,
                        e.idCargo,
                        CargoNombre = c.nombreCargo
                    }
                ).FirstOrDefaultAsync();

                if (empleadoConCargo == null)
                    throw new Exception("Empleado no encontrado.");

                bool esVendedor = empleadoConCargo.CargoNombre != null &&
                                  empleadoConCargo.CargoNombre.ToLower().Contains("vendedor");

                if (!esVendedor)
                {
                    decimal salarioDiario = empleadoConCargo.salarioDiario;

                    switch (remuneracionDto.idTipoRemuneracion)
                    {
                        case 1: // Horas Extra
                            remuneracionDto.pagoQuincenal = remuneracionDto.horas.HasValue
                                ? remuneracionDto.horas.Value * empleadoConCargo.salarioPorHoraExtra
                                : 0;
                            break;

                        case 2: // Día Feriado
                            remuneracionDto.pagoQuincenal = remuneracionDto.TrabajoEnDia
                                ? salarioDiario
                                : 0;  // Si no trabajó, no paga nada
                            break;

                        case 3: // Incapacidad (mitad salario * días incapacidad, máximo 3 días)
                            int diasIncapacidad = remuneracionDto.diasTrabajados.HasValue
                                ? (15 - remuneracionDto.diasTrabajados.Value) // Suponiendo 15 días quincena
                                : 0;

                            if (diasIncapacidad > 3) diasIncapacidad = 3;
                            if (diasIncapacidad < 0) diasIncapacidad = 0;

                            remuneracionDto.pagoQuincenal = (salarioDiario / 2) * diasIncapacidad;
                            break;

                        case 4: // Maternidad (mitad de salario por quincena)
                            remuneracionDto.pagoQuincenal = salarioDiario * 15 / 2;
                            break;

                        case 5: // Vacaciones
                            remuneracionDto.pagoQuincenal = remuneracionDto.TrabajoEnDia && remuneracionDto.diasTrabajados.HasValue
                                ? salarioDiario * remuneracionDto.diasTrabajados.Value
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
                }

                Remuneracion remuneracion = ConvertirDtoAEntidad(remuneracionDto, esVendedor);
                _contexto.Remuneracion.Add(remuneracion);
                return await _contexto.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar la remuneración manual: " + ex.Message);
            }
        }

        private Remuneracion ConvertirDtoAEntidad(RemuneracionDto dto, bool esVendedor = false)
        {
            if (esVendedor)
            {
                return new Remuneracion
                {
                    idEmpleado = dto.idEmpleado,
                    idTipoRemuneracion = dto.idTipoRemuneracion,
                    fechaRemuneracion = dto.fechaRemuneracion,
                    comision = dto.comision,
                    pagoQuincenal = dto.pagoQuincenal,
                    idEstado = dto.idEstado
                };
            }
            else
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
}
