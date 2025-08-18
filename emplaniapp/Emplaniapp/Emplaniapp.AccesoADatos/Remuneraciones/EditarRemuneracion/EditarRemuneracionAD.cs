using System.Data.Entity;
using System.Linq;
using Emplaniapp.Abstracciones.InterfacesAD.Remuneraciones.EditarRemuneracion;
using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.Abstracciones.ModelosParaUI;

namespace Emplaniapp.AccesoADatos.Remuneraciones.EditarRemuneracion
{
    public class EditarRemuneracionAD : IEditarRemuneracionAD
    {
        private Contexto _contexto;

        public EditarRemuneracionAD()
        {
            _contexto = new Contexto();
        }

        public int Actualizar(RemuneracionDto laRemuneracion)
        {
            var laRemuneracionEnBaseDeDatos = _contexto.Remuneracion
                .FirstOrDefault(r => r.idRemuneracion == laRemuneracion.idRemuneracion);

            if (laRemuneracionEnBaseDeDatos == null)
                return 0;

            // Obtener datos del empleado junto con el nombre del cargo usando join
            var datosEmpleadoConCargo = (from e in _contexto.Empleados
                                         join c in _contexto.Cargos on e.idCargo equals c.idCargo
                                         where e.idEmpleado == laRemuneracion.idEmpleado
                                         select new
                                         {
                                             e.salarioDiario,
                                             e.salarioPorHoraExtra,
                                             CargoNombre = c.nombreCargo
                                         }).FirstOrDefault();

            if (datosEmpleadoConCargo == null)
                return 0;

            bool esVendedor = datosEmpleadoConCargo.CargoNombre != null &&
                              datosEmpleadoConCargo.CargoNombre.ToLower().Contains("vendedor");

            if (esVendedor)
            {
                // Solo asignar comisión y pagoQuincenal como vienen
                laRemuneracionEnBaseDeDatos.comision = laRemuneracion.comision;
                laRemuneracionEnBaseDeDatos.pagoQuincenal = laRemuneracion.pagoQuincenal;
                // Opcional: podrías limpiar campos no usados para vendedor
                laRemuneracionEnBaseDeDatos.diasTrabajados = null;
                laRemuneracionEnBaseDeDatos.horas = null;
            }
            else
            {
                // Para no vendedores, realizar cálculo de pago
                decimal pago = 0;
                decimal salarioDiario = datosEmpleadoConCargo.salarioDiario;
                decimal salarioPorHoraExtra = datosEmpleadoConCargo.salarioPorHoraExtra;

                laRemuneracionEnBaseDeDatos.diasTrabajados = laRemuneracion.diasTrabajados;
                laRemuneracionEnBaseDeDatos.horas = laRemuneracion.horas;
                laRemuneracionEnBaseDeDatos.comision = laRemuneracion.comision;

                switch (laRemuneracion.idTipoRemuneracion)
                {
                    case 1: // Horas Extra
                        pago = laRemuneracion.horas.HasValue
                            ? laRemuneracion.horas.Value * salarioPorHoraExtra
                            : 0;
                        break;

                    case 2: // Día Feriado
                        pago = laRemuneracion.TrabajoEnDia ? salarioDiario : 0;
                        break;

                    case 3: // Incapacidad
                        int diasIncapacidad = laRemuneracion.diasTrabajados.HasValue
                            ? (15 - laRemuneracion.diasTrabajados.Value) // asumiendo 15 días por quincena
                            : 0;

                        if (diasIncapacidad > 3) diasIncapacidad = 3;
                        if (diasIncapacidad < 0) diasIncapacidad = 0;

                        pago = (salarioDiario / 2) * diasIncapacidad;
                        break;

                    case 4: // Maternidad
                        pago = (salarioDiario * 15) / 2;
                        break;

                    case 5: // Vacaciones
                        pago = laRemuneracion.TrabajoEnDia && laRemuneracion.diasTrabajados.HasValue
                            ? salarioDiario * laRemuneracion.diasTrabajados.Value
                            : 0;
                        break;

                    case 6: // Pago Quincenal
                        pago = laRemuneracion.diasTrabajados.HasValue
                            ? laRemuneracion.diasTrabajados.Value * salarioDiario
                            : 0;
                        break;

                    default:
                        pago = 0;
                        break;
                }

                laRemuneracionEnBaseDeDatos.pagoQuincenal = pago;
            }

            _contexto.Entry(laRemuneracionEnBaseDeDatos).State = System.Data.Entity.EntityState.Modified;
            return _contexto.SaveChanges();
        }
    }
}
