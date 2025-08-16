using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.General.OtenerTotalEmpleados;

namespace Emplaniapp.AccesoADatos.General.ObtenerTotalEmpleados
{
    public class obtenerTotalEmpleadosAD : IObtenerTotalEmpleadosAD
    {
        private Contexto _contexto;

        public obtenerTotalEmpleadosAD()
        {
            _contexto = new Contexto();
        }

        public int ObtenerTotalEmpleados(string filtro, int? idCargo, int? idEstado, bool soloActivos = false)
        {
            // Consulta base con joins necesarios
            var query = from empleado in _contexto.Empleados.AsNoTracking()
                        join cargo in _contexto.Cargos on empleado.idCargo equals cargo.idCargo
                        join estado in _contexto.Estado on empleado.idEstado equals estado.idEstado
                        select new { empleado, cargo, estado };

            // Aplicar filtros básicos que se pueden ejecutar en SQL
            if (idEstado.HasValue || soloActivos)
            {
                query = query.Where(x => soloActivos ? x.empleado.idEstado == 1 : x.empleado.idEstado == idEstado.Value);
            }

            if (idCargo.HasValue)
            {
                query = query.Where(x => x.empleado.idCargo == idCargo.Value);
            }

            // Traer datos a memoria para filtros complejos
            var empleados = query.ToList();

            // Aplicar filtro de texto en memoria
            if (!string.IsNullOrEmpty(filtro))
            {
                filtro = filtro.ToLower();
                empleados = empleados.Where(x =>
                {
                    // Construcción optimizada del nombre completo
                    var nombreCompleto = new StringBuilder();
                    if (!string.IsNullOrWhiteSpace(x.empleado.nombre)) nombreCompleto.Append(x.empleado.nombre.ToLower() + " ");
                    if (!string.IsNullOrWhiteSpace(x.empleado.segundoNombre)) nombreCompleto.Append(x.empleado.segundoNombre.ToLower() + " ");
                    if (!string.IsNullOrWhiteSpace(x.empleado.primerApellido)) nombreCompleto.Append(x.empleado.primerApellido.ToLower() + " ");
                    if (!string.IsNullOrWhiteSpace(x.empleado.segundoApellido)) nombreCompleto.Append(x.empleado.segundoApellido.ToLower());

                    return nombreCompleto.ToString().Contains(filtro) ||
                           x.empleado.cedula.ToString().Contains(filtro);
                }).ToList();
            }

            return empleados.Count;
        }
    }
}
