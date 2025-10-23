using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.General.FiltrarEmpleados;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.AccesoADatos.General.Filtrar
{
    public class filtrarEmpleadosAD : IFiltrarEmpleadosAD
    {
        private Contexto _contexto;

        public filtrarEmpleadosAD()
        {
            _contexto = new Contexto();
        }
        public List<T> ObtenerFiltrado<T>(string filtro, int? idCargo, int? idEstado) where T : class
        {
            return ObtenerFiltrado<T>(filtro, idCargo, idEstado, null);
        }

        public List<T> ObtenerFiltrado<T>(string filtro, int? idCargo, int? idEstado, string usuarioActualId = null) where T : class
        {
            var query = from empleado in _contexto.Empleados.AsNoTracking()
                        join estado in _contexto.Estado on empleado.idEstado equals estado.idEstado
                        join cargo in _contexto.Cargos on empleado.idCargo equals cargo.idCargo
                        where (idEstado == null || empleado.idEstado == idEstado)
                        // ✨ MEJORA: Excluir el usuario actual del filtrado por seguridad
                        && (usuarioActualId == null || empleado.IdNetUser != usuarioActualId)
                        select new { empleado, estado, cargo };

            // Aplicar filtros de ID
            if (idCargo.HasValue)
            {
                query = query.Where(x => x.empleado.idCargo == idCargo.Value);
            }

            // Traer datos a memoria antes de aplicar filtros complejos
            var empleadosFiltrados = query.ToList();

            // Aplicar filtro de texto en memoria
            if (!string.IsNullOrEmpty(filtro))
            {
                filtro = filtro.ToLower();
                empleadosFiltrados = empleadosFiltrados.Where(x =>
                {
                    // Construir nombre completo en memoria
                    var nombreCompleto = new List<string>();
                    if (!string.IsNullOrWhiteSpace(x.empleado.nombre)) nombreCompleto.Add(x.empleado.nombre);
                    if (!string.IsNullOrWhiteSpace(x.empleado.segundoNombre)) nombreCompleto.Add(x.empleado.segundoNombre);
                    if (!string.IsNullOrWhiteSpace(x.empleado.primerApellido)) nombreCompleto.Add(x.empleado.primerApellido);
                    if (!string.IsNullOrWhiteSpace(x.empleado.segundoApellido)) nombreCompleto.Add(x.empleado.segundoApellido);

                    var nombreConcatenado = string.Join(" ", nombreCompleto).ToLower();

                    return nombreConcatenado.Contains(filtro) ||
                           x.empleado.cedula.ToString().Contains(filtro);
                }).ToList();
            }

            var idsFiltrados = empleadosFiltrados.Select(x => x.empleado.idEmpleado).ToList();

            if (typeof(T) == typeof(HojaResumenDto))
            {
                return empleadosFiltrados.Select(item => new HojaResumenDto
                {
                    IdEmpleado = item.empleado.idEmpleado,
                    NombreEmpleado = string.Join(" ", new[]
                    {
                    item.empleado.nombre,
                    item.empleado.segundoNombre,
                    item.empleado.primerApellido,
                    item.empleado.segundoApellido
                }.Where(n => !string.IsNullOrWhiteSpace(n))),
                    Cedula = item.empleado.cedula,
                    NombrePuesto = item.cargo.nombreCargo,
                    SalarioAprobado = item.empleado.salarioAprobado,
                    TotalRemuneraciones = _contexto.Remuneracion
                                        .Where(r => r.idEmpleado == item.empleado.idEmpleado && r.idEstado == 1)
                                        .Sum(r => r.pagoQuincenal) ?? 0m,
                    TotalRetenciones = _contexto.Retenciones
                                        .Where(rt => rt.idEmpleado == item.empleado.idEmpleado && rt.idEstado == 1)
                                        .Sum(rt => rt.rebajo) ?? 0m,
                    MontoLiquidacion = _contexto.Liquidaciones
                                        .Where(l => l.idEmpleado == item.empleado.idEmpleado)
                                        .Select(l => l.costoLiquidacion)
                                        .FirstOrDefault(),
                    FechaLiquidacion = _contexto.Liquidaciones
                                        .Where(l => l.idEmpleado == item.empleado.idEmpleado)
                                        .Select(l => l.fechaLiquidacion)
                                        .FirstOrDefault(),
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
                } as T).ToList();
            }
            else if (typeof(T) == typeof(EmpleadoDto))
            {
                // Consulta optimizada para obtener solo los empleados filtrados con todos los datos necesarios
                var empleadosCompletos = (from emp in _contexto.Empleados.AsNoTracking()
                                          join estado in _contexto.Estado on emp.idEstado equals estado.idEstado
                                          join cargo in _contexto.Cargos on emp.idCargo equals cargo.idCargo
                                          join banco in _contexto.Bancos on emp.idBanco equals banco.idBanco
                                          join moneda in _contexto.TipoMoneda on emp.idTipoMoneda equals moneda.idTipoMoneda
                                          join prov in _contexto.Provincia on emp.idProvincia equals prov.idProvincia
                                          join cant in _contexto.Canton on emp.idCanton equals cant.idCanton
                                          join dist in _contexto.Distrito on emp.idDistrito equals dist.idDistrito
                                          where idsFiltrados.Contains(emp.idEmpleado)
                                          orderby emp.primerApellido, emp.segundoApellido, emp.nombre
                                          select new
                                          {
                                              emp,
                                              estado,
                                              cargo,
                                              banco,
                                              moneda,
                                              prov,
                                              cant,
                                              dist
                                          }).ToList();

                return empleadosCompletos.Select(emp => new EmpleadoDto
                {
                    idEmpleado = emp.emp.idEmpleado,
                    nombre = emp.emp.nombre,
                    segundoNombre = emp.emp.segundoNombre ?? string.Empty,
                    primerApellido = emp.emp.primerApellido,
                    segundoApellido = emp.emp.segundoApellido,
                    cedula = emp.emp.cedula,
                    fechaNacimiento = emp.emp.fechaNacimiento,
                    numeroTelefonico = emp.emp.numeroTelefonico,
                    correoInstitucional = emp.emp.correoInstitucional,
                    idEstado = emp.emp.idEstado,
                    nombreEstado = emp.estado.nombreEstado,
                    idCargo = emp.emp.idCargo,
                    nombreCargo = emp.cargo.nombreCargo,
                    salarioAprobado = emp.emp.salarioAprobado,
                    periocidadPago = emp.emp.periocidadPago,
                    nombreMoneda = emp.moneda.nombreMoneda,
                    cuentaIBAN = emp.emp.cuentaIBAN,
                    idBanco = emp.emp.idBanco,
                    nombreBanco = emp.banco.nombreBanco,
                    direccionCompleta = $"{emp.prov.nombreProvincia}, {emp.cant.nombreCanton}, {emp.dist.nombreDistrito}, {emp.emp.direccionDetallada}",
                    fechaContratacion = emp.emp.fechaContratacion,
                    fechaSalida = emp.emp.fechaSalida
                } as T).ToList();
            }

            throw new NotSupportedException($"Tipo {typeof(T).Name} no soportado");
        }
    }
}


