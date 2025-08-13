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

            Remuneracion laRemuneracionEnBaseDeDatos = _contexto.Remuneracion.Where(Remuneracion => Remuneracion.idRemuneracion == laRemuneracion.idRemuneracion).FirstOrDefault();
            var datosEmpleado = _contexto.Empleados
            .FirstOrDefault(e => e.idEmpleado == laRemuneracion.idEmpleado);

            if (datosEmpleado == null)
                return 0;
            decimal salarioDiario = datosEmpleado.salarioDiario;
            decimal salarioPorHoraExtra = datosEmpleado.salarioPorHoraExtra;
            decimal pago = 0;

            laRemuneracion.diasTrabajados = laRemuneracion.diasTrabajados;
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
                    pago = laRemuneracion.TrabajoEnDia ? salarioDiario * 2 : salarioDiario;
                    break;

                case 3: // Incapacidad
                    pago = (salarioDiario / 2) * 3;
                    break;

                case 4: // Maternidad
                    pago = (salarioDiario * 15) / 2;
                    break;

                case 5: // Vacaciones
                    pago = laRemuneracion.TrabajoEnDia ? salarioDiario : 0;
                    break;

                case 6: // Pago Quincenal
                    pago = laRemuneracion.diasTrabajados.HasValue
                        ? laRemuneracion.diasTrabajados.Value * salarioDiario
                        : 0;
                    break;
            }

            // Asignar pago calculado a la entidad
            laRemuneracionEnBaseDeDatos.pagoQuincenal = pago;
            EntityState estado = _contexto.Entry(laRemuneracionEnBaseDeDatos).State = System.Data.Entity.EntityState.Modified;
            int cantidadDatosAgregados = _contexto.SaveChanges();
            return cantidadDatosAgregados;
        }
    }
}
