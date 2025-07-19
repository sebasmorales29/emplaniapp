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
            if (remuneracionDto.idTipoRemuneracion == 1)
            {
                var empleado = await _contexto.Empleados
                    .Where(e => e.idEmpleado == remuneracionDto.idEmpleado)
                    .Select(e => new { e.salarioPorHoraExtra })
                    .FirstOrDefaultAsync();

                if (empleado != null)
                {
                    remuneracionDto.pagoQuincenal = remuneracionDto.horas * empleado.salarioPorHoraExtra;
                }
                else
                {
                    throw new Exception("No se encontró el empleado para calcular el pago por horas extra.");
                }
            }
            Remuneracion laRemuneracionAGuardar = ConvertirDtoAEntidad(remuneracionDto);
            _contexto.Remuneracion.Add(laRemuneracionAGuardar);
            int cantidadDatosAgregados = await _contexto.SaveChangesAsync();
            return cantidadDatosAgregados;
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
