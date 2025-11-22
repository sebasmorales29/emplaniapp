using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using Emplaniapp.Abstracciones.InterfacesAD.Hoja_Resumen.ListarHojaResumen;
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
           
            var hoy = DateTime.Today;
            var inicioQuincena = hoy.Day <= 15
                ? new DateTime(hoy.Year, hoy.Month, 1)
                : new DateTime(hoy.Year, hoy.Month, 16);
            var finQuincena = hoy.Day <= 15
                ? new DateTime(hoy.Year, hoy.Month, 15)
                : new DateTime(hoy.Year, hoy.Month, DateTime.DaysInMonth(hoy.Year, hoy.Month));

            var empleados = (from empleado in _contexto.Empleados.AsNoTracking()
                             join cargos in _contexto.Cargos on empleado.idCargo equals cargos.idCargo
                             where empleado.idEstado == 1 && empleado.salarioAprobado != 0
                             select new { empleado, cargos }).ToList(); // Traer datos a memoria

            var hojasResumen = empleados.Select(e => new HojaResumenDto
            {
                IdEmpleado = e.empleado.idEmpleado,

                Cedula = e.empleado.cedula,

                NombreEmpleado = string.Join(" ", new[] {
                    e.empleado.nombre,
                    e.empleado.segundoNombre,
                    e.empleado.primerApellido,
                    e.empleado.segundoApellido
                    }.Where(n => !string.IsNullOrWhiteSpace(n))),

                NombrePuesto = e.cargos.nombreCargo,

                // Datos Financieros
                SalarioAprobado = e.empleado.salarioAprobado,

                TotalRemuneraciones = _contexto.Remuneracion
                .Where(r => r.idEmpleado == e.empleado.idEmpleado
                    && r.idEstado == 1
                    && r.fechaRemuneracion >= inicioQuincena && r.fechaRemuneracion <= finQuincena)
                .Sum(r => r.pagoQuincenal) ?? 0m,
                    TotalRetenciones = _contexto.Retenciones
                .Where(rt => rt.idEmpleado == e.empleado.idEmpleado
                    && rt.idEstado == 1
                    && rt.fechaRetencion >= inicioQuincena && rt.fechaRetencion <= finQuincena)
                .Sum(rt => rt.rebajo) ?? 0m,

                MontoLiquidacion = _contexto.Liquidaciones
                .Where(l => l.idEmpleado == e.empleado.idEmpleado
                    && l.fechaLiquidacion >= inicioQuincena && l.fechaLiquidacion <= finQuincena
                    && l.idEstado==1)
                .Select(l => l.costoLiquidacion)
                .FirstOrDefault(),

                FechaLiquidacion = _contexto.Liquidaciones
                .Where(l => l.idEmpleado == e.empleado.idEmpleado
                    && l.fechaLiquidacion >= inicioQuincena && l.fechaLiquidacion <= finQuincena)
                .Select(l => l.fechaLiquidacion)
                .FirstOrDefault(),

                SalarioNeto = _contexto.PagoQuincenal
                .Where(p => p.idEmpleado == e.empleado.idEmpleado
                    && p.fechaFin >= inicioQuincena && p.fechaFin <= finQuincena)
                .OrderByDescending(p => p.fechaFin)
                .Select(p => p.salarioNeto)
                .FirstOrDefault(),

                IdPagoQuincenal = _contexto.PagoQuincenal
                .Where(p => p.idEmpleado == e.empleado.idEmpleado
                    && p.fechaFin >= inicioQuincena
                    && p.fechaFin <= finQuincena)
                .Select(p => (int)p.idPagoQuincenal) // si es nullable
                .FirstOrDefault(),

                Aprobado = _contexto.PagoQuincenal
                .Where(p => p.idEmpleado == e.empleado.idEmpleado
                    && p.fechaFin >= inicioQuincena && p.fechaFin <= finQuincena)
                .OrderByDescending(p => p.fechaFin)
                .Select(p => p.aprobacion)
                .FirstOrDefault()

               
            }).ToList();
            return hojasResumen;
        }

    }
}
