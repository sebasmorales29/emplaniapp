using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.HojaResumen;
using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.AccesoADatos.Hoja_Resumen
{
    public class listarHojaResumenAD : IListarHojaResumenAD
    {
        private Contexto _contexto;

        public listarHojaResumenAD()
        {
            _contexto = new Contexto();
        }

        public List<HojaResumenDto> ObtenerHojasResumen()
        {
            var hojasResumen = (from empleado in _contexto.Empleado.AsNoTracking()
                                join estado in _contexto.Estado on empleado.idEstado equals estado.idEstado
                                join cargos in _contexto.Cargos on empleado.idCargo equals cargos.idCargo
                                select new HojaResumenDto
                                {
                                    IdEmpleado = empleado.idEmpleado,
                                    Cedula = empleado.cedula,
                                    NombreEmpleado = empleado.nombre + " " + empleado.segundoNombre + " " + empleado.primerApellido + " " + empleado.segundoApellido,
                                    NombrePuesto = cargos.nombreCargo,

                                    // Datos Financieros
                                    SalarioAprobado = empleado.salarioAprobado,
                                    TotalRemuneraciones = _contexto.Remuneracion
                                        .Where(r => r.idEmpleado == empleado.idEmpleado && r.idEstado == 1)
                                        .Sum(r => r.pagoQuincenal) ?? 0m,
                                    TotalRetenciones = _contexto.Retenciones
                                        .Where(rt => rt.idEmpleado == empleado.idEmpleado && rt.idEstado == 1)
                                        .Sum(rt => rt.rebajo) ?? 0m,

                                    MontoLiquidacion = _contexto.Liquidaciones
                                        .Where(l => l.idEmpleado == empleado.idEmpleado)
                                        .Select(l => l.costoLiquidacion)
                                        .FirstOrDefault(),
                                    FechaLiquidacion = _contexto.Liquidaciones
                                        .Where(l => l.idEmpleado == empleado.idEmpleado)
                                        .Select(l => l.fechaLiquidacion)
                                        .FirstOrDefault(),

                                    SalarioNeto = _contexto.PagoQuincenal
                                        .Where(p => p.idEmpleado == empleado.idEmpleado)
                                        .OrderByDescending(p => p.fechaFin)
                                        .Select(p => p.salarioNeto)
                                        .FirstOrDefault(),

                                    idEstado = estado.idEstado,
                                    nombreEstado = estado.nombreEstado,

                                    Aprobado = _contexto.PagoQuincenal
                                        .Where(p => p.idEmpleado == empleado.idEmpleado)
                                        .OrderByDescending(p => p.fechaFin)
                                        .Select(p => p.aprobacion)
                                        .FirstOrDefault()
                                }).ToList();

            return hojasResumen;
        }

        public List<HojaResumenDto> ObtenerFiltrado(string filtro, int? idCargo)
        {
            var query = from empleado in _contexto.Empleado.AsNoTracking()
                        join estado in _contexto.Estado on empleado.idEstado equals estado.idEstado
                        join cargos in _contexto.Cargos on empleado.idCargo equals cargos.idCargo
                        where estado.idEstado == 1
                        select new { empleado, cargos, estado };

            if (!string.IsNullOrEmpty(filtro))
            {
                filtro = filtro.ToLower();

                query = query.Where(x =>
                    (
                        (x.empleado.nombre + " " +
                         x.empleado.segundoNombre + " " +
                         x.empleado.primerApellido + " " +
                         x.empleado.segundoApellido
                        ).Trim().ToLower().Contains(filtro)
                    ) ||
                    x.empleado.cedula.ToString().Contains(filtro)
                );
            }

            if (idCargo.HasValue)
            {
                query = query.Where(x => x.empleado.idCargo == idCargo.Value);
            }

            var empleados = query.ToList();

            var resultado = empleados.Select(item => new HojaResumenDto
            {
                IdEmpleado = item.empleado.idEmpleado,
                NombreEmpleado = item.empleado.nombre + " " + item.empleado.segundoNombre + " " + item.empleado.primerApellido + " " + item.empleado.segundoApellido,
                Cedula = item.empleado.cedula,
                NombrePuesto = item.cargos.nombreCargo,

                SalarioAprobado = item.empleado.salarioAprobado,
                TotalRemuneraciones = _contexto.Remuneracion
                                        .Where(r => r.idEmpleado == item.empleado.idEmpleado && r.idEstado == 1)
                                        .Sum(r => r.pagoQuincenal) ?? 0m,
                TotalRetenciones = _contexto.Retenciones
                                        .Where(rt => rt.idEmpleado == item.empleado.idEmpleado && rt.idEstado == 1)
                                        .Sum(rt => rt.rebajo) ?? 0m,

                // Liquidación
                MontoLiquidacion = _contexto.Liquidaciones
                                        .Where(l => l.idEmpleado == item.empleado.idEmpleado)
                                        .Select(l => l.costoLiquidacion)
                                        .FirstOrDefault(),
                FechaLiquidacion = _contexto.Liquidaciones
                                        .Where(l => l.idEmpleado == item.empleado.idEmpleado)
                                        .Select(l => l.fechaLiquidacion)
                                        .FirstOrDefault(),

                // Salario Neto (último registro)
                SalarioNeto = _contexto.PagoQuincenal
                                        .Where(p => p.idEmpleado == item.empleado.idEmpleado)
                                        .OrderByDescending(p => p.fechaFin)
                                        .Select(p => p.salarioNeto)
                                        .FirstOrDefault(),

                // Información del estado
                idEstado = item.estado.idEstado,
                nombreEstado = item.estado.nombreEstado,

                Aprobado = _contexto.PagoQuincenal
                                        .Where(p => p.idEmpleado == item.empleado.idEmpleado)
                                        .OrderByDescending(p => p.fechaFin)
                                        .Select(p => p.aprobacion)
                                        .FirstOrDefault()

            }).ToList();

            return resultado;
        }


        public List<CargoDto> ObtenerCargos()
        {
            return _contexto.Cargos
                .Select(p => new CargoDto
                {
                    idCargo = p.idCargo,
                    nombreCargo = p.nombreCargo
                }).ToList();
        }
    }
}
