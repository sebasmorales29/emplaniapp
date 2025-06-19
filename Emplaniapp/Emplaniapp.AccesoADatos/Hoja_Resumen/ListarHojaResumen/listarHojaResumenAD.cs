using System.Collections.Generic;
using System.Linq;
using Emplaniapp.Abstracciones.InterfacesAD.Hoja_Resumen.ListarHojaResumen;
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
            var empleados = (from empleado in _contexto.Empleados.AsNoTracking()
                                join cargos in _contexto.Cargos on empleado.idCargo equals cargos.idCargo
                                where empleado.idEstado == 1
                                select new { empleado, cargos }).ToList(); // Traer los datos a memoria

            var hojasResumen = empleados.Select(e => new HojaResumenDto
                                {
                                    IdEmpleado = e.empleado.idEmpleado,
                                    Cedula = e.empleado.cedula,
                                    NombreEmpleado = string.Join(" ", new[] { e.empleado.nombre, e.empleado.segundoNombre, e.empleado.primerApellido, e.empleado.segundoApellido }.Where(n => !string.IsNullOrWhiteSpace(n))),
                                    NombrePuesto = e.cargos.nombreCargo,

                                    // Datos Financieros
                                    SalarioAprobado = e.empleado.salarioAprobado,
                                    TotalRemuneraciones = _contexto.Remuneracion
                                        .Where(r => r.idEmpleado == e.empleado.idEmpleado && r.idEstado == 1)
                                        .Sum(r => r.pagoQuincenal) ?? 0m,
                                    TotalRetenciones = _contexto.Retenciones
                                        .Where(rt => rt.idEmpleado == e.empleado.idEmpleado && rt.idEstado == 1)
                                        .Sum(rt => rt.rebajo) ?? 0m,

                                    MontoLiquidacion = _contexto.Liquidaciones
                                        .Where(l => l.idEmpleado == e.empleado.idEmpleado)
                                        .Select(l => l.costoLiquidacion)
                                        .FirstOrDefault(),
                                    FechaLiquidacion = _contexto.Liquidaciones
                                        .Where(l => l.idEmpleado == e.empleado.idEmpleado)
                                        .Select(l => l.fechaLiquidacion)
                                        .FirstOrDefault(),

                                    SalarioNeto = _contexto.PagoQuincenal
                                        .Where(p => p.idEmpleado == e.empleado.idEmpleado)
                                        .OrderByDescending(p => p.fechaFin)
                                        .Select(p => p.salarioNeto)
                                        .FirstOrDefault(),

                                    Aprobado = _contexto.PagoQuincenal
                                        .Where(p => p.idEmpleado == e.empleado.idEmpleado)
                                        .OrderByDescending(p => p.fechaFin)
                                        .Select(p => p.aprobacion)
                                        .FirstOrDefault()
                                }).ToList();

            return hojasResumen;
        }

        public List<HojaResumenDto> ObtenerFiltrado(string filtro, int? idCargo)
        {
            var query = from empleado in _contexto.Empleados.AsNoTracking()
                        join cargos in _contexto.Cargos on empleado.idCargo equals cargos.idCargo
                        where empleado.idEstado == 1
                        select new { empleado, cargos };

            // Aplicar filtros de ID que SÍ se pueden traducir a SQL
            if (idCargo.HasValue)
            {
                query = query.Where(x => x.empleado.idCargo == idCargo.Value);
            }

            var empleados = query.ToList(); // Traer los datos filtrados por ID a memoria

            // Aplicar el filtro de texto AHORA, en memoria, donde podemos hacer la concatenación compleja.
            if (!string.IsNullOrEmpty(filtro))
            {
                filtro = filtro.ToLower();
                empleados = empleados.Where(x =>
                    (
                        string.Join(" ", new[] { x.empleado.nombre, x.empleado.segundoNombre, x.empleado.primerApellido, x.empleado.segundoApellido }.Where(n => !string.IsNullOrWhiteSpace(n)))
                        .ToLower().Contains(filtro)
                    ) ||
                    x.empleado.cedula.ToString().Contains(filtro)
                ).ToList();
            }

            var resultado = empleados.Select(item => new HojaResumenDto
            {
                IdEmpleado = item.empleado.idEmpleado,
                NombreEmpleado = string.Join(" ", new[] { item.empleado.nombre, item.empleado.segundoNombre, item.empleado.primerApellido, item.empleado.segundoApellido }.Where(n => !string.IsNullOrWhiteSpace(n))),
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

                Aprobado = _contexto.PagoQuincenal
                                        .Where(p => p.idEmpleado == item.empleado.idEmpleado)
                                        .OrderByDescending(p => p.fechaFin)
                                        .Select(p => p.aprobacion)
                                        .FirstOrDefault()

            }).ToList();

            return resultado;
        }
        public int ObtenerTotalEmpleados(string filtro, int? idCargo)
        {
            var query = from empleado in _contexto.Empleados.AsNoTracking()
                        join cargos in _contexto.Cargos on empleado.idCargo equals cargos.idCargo
                        select new { empleado, cargos };

            // Aplicar filtro por cargo directamente en base de datos
            if (idCargo.HasValue)
            {
                query = query.Where(x => x.empleado.idCargo == idCargo.Value);
            }

            // Obtener los datos en memoria
            var total = query.ToList();

            // Filtrar por texto en memoria (después de traer los datos)
            if (!string.IsNullOrEmpty(filtro))
            {
                filtro = filtro.ToLower();

                total = total.Where(x =>
                    string.Join(" ", new[]
                        {
                    x.empleado.nombre,
                    x.empleado.segundoNombre,
                    x.empleado.primerApellido,
                    x.empleado.segundoApellido
                        }
                        .Where(n => !string.IsNullOrWhiteSpace(n)))
                    .ToLower().Contains(filtro) ||
                    x.empleado.cedula.ToString().Contains(filtro)
                ).ToList();
            }

            return total.Count();
        }
    }
}
