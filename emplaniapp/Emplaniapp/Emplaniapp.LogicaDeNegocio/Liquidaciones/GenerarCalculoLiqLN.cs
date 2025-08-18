using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.Abstracciones.InterfacesParaUI.Liquidaciones;
using Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.LogicaDeNegocio.Empleado.ObtenerEmpleadoPorId;
using Emplaniapp.LogicaDeNegocio.Remuneraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.LogicaDeNegocio.Liquidaciones
{
    public class GenerarCalculoLiqLN : IGenerarCalculoLiqLN
    {
        IObtenerEmpleadoPorIdLN _empleado;
        IListarRemuneracionesLN _remuneraciones;

        public GenerarCalculoLiqLN() { 
            _empleado = new ObtenerEmpleadoPorIdLN();
            _remuneraciones = new ListarRemuneracionesLN();
        }

        // Generar cálculo
        public LiquidacionDto PrimerCalculo(EmpleadoDto emp, DateTime fechaliq, string motivo, string observs)
        {
            // 1. cálculo del Tiempo de trabajo
            int aniosTrab = fechaliq.Year - emp.fechaContratacion.Year;
            int mesesTrab = mesesDeTrabajo(emp.fechaContratacion, fechaliq);

            // 2. Salario Promedio 
            decimal salProm = calcularSalarioProm(emp.idEmpleado, emp.salarioPorHoraExtra, mesesTrab, fechaliq);

            // 3. Preaviso
            int diasPreav = diasPreaviso(mesesTrab);
            string fechaPrev = fechaDePreaviso(fechaliq, diasPreav);
            decimal preaviso = calculoPreaviso(motivo, salProm);

            // 4. Aguinaldo Proporcional 
            decimal aguinaldoProp = calculoAguinaldoP(mesesTrab, fechaliq, salProm);

            // 5. Cesantía 
            decimal cesantia = pagoCesantia(motivo, mesesTrab, emp.salarioDiario, aniosTrab);

            // 6. Remuneraciones pendientes: vacaciones, pagos no hechos hasta el momento
            int diasVac = diasVacaciones(emp);
            decimal vacFaltantes = vacasFaltantes(diasPreav ,emp.salarioDiario);
            decimal pagosFaltantes = remuneracionesPendientes(emp.idEmpleado, fechaliq, emp.salarioDiario);

            // Costo de Liquidación
            decimal costoLiq = preaviso + aguinaldoProp + vacFaltantes + cesantia + pagosFaltantes;

            //decimal salariosPendientes = remuneracionesPendientes(emp.idEmpleado, fechaliq);
            LiquidacionDto liquid = new LiquidacionDto
            {
                idEmpleado = emp.idEmpleado,

                fechaLiquidacion = fechaliq,
                motivoLiquidacion = motivo,

                salarioPromedio = salProm,
                aniosAntiguedad = aniosTrab,
                diasPreaviso = diasPreav,
                fechaPreaviso = fechaPrev,
                diasVacacionesPendientes = diasVac, // Nota, estoy hay que hablarlo con el grupo

                pagoPreaviso = preaviso,
                pagoAguinaldoProp = aguinaldoProp,
                pagoVacacionesNG = vacFaltantes,
                pagoCesantia = cesantia,
                remuPendientes = pagosFaltantes, // preguntar como se hizo
                costoLiquidacion = costoLiq,

                observacionLiquidacion = observs,
                idEstado = 2
            };

            return liquid;

        }

        public LiquidacionDto ModificarCalculo(LiquidacionDto liq)
        {
            // Se extraen datos
            DateTime fechaliq = liq.fechaLiquidacion;
            EmpleadoDto emp = _empleado.ObtenerEmpleadoPorId(liq.idEmpleado);
            string motivo = liq.motivoLiquidacion.ToString();
            string observs = liq.observacionLiquidacion;

            //  CALCULOS -------------------------------------------------------------

            // 1. cálculo del Tiempo de trabajo
            int aniosTrab = fechaliq.Year - emp.fechaContratacion.Year;
            int mesesTrab = mesesDeTrabajo(emp.fechaContratacion, fechaliq);

            // 2. Salario Promedio 
            decimal salProm = calcularSalarioProm(emp.idEmpleado, emp.salarioPorHoraExtra, mesesTrab, fechaliq);

            // 3. Preaviso
            int diasPreav = diasPreaviso(mesesTrab);
            string fechaPrev = fechaDePreaviso(fechaliq, diasPreav);
            decimal preaviso = calculoPreaviso(motivo, salProm);

            // 4. Aguinaldo Proporcional 
            decimal aguinaldoProp = calculoAguinaldoP(mesesTrab, fechaliq, salProm);

            // 5. Cesantía 
            decimal cesantia = pagoCesantia(motivo, mesesTrab, emp.salarioDiario, aniosTrab);

            // 6. Remuneraciones pendientes: vacaciones, pagos no hechos hasta el momento
            int diasVac = diasVacaciones(emp);
            decimal vacFaltantes = vacasFaltantes(diasPreav, emp.salarioDiario);
            decimal pagosFaltantes = remuneracionesPendientes(emp.idEmpleado, fechaliq, emp.salarioDiario);

            // Costo de Liquidación
            decimal costoLiq = preaviso + aguinaldoProp + vacFaltantes +cesantia + pagosFaltantes;

            //decimal salariosPendientes = remuneracionesPendientes(emp.idEmpleado, fechaliq);
            LiquidacionDto liquid = new LiquidacionDto
            {
                idLiquidacion = liq.idLiquidacion,
                idEmpleado = emp.idEmpleado,

                fechaLiquidacion = fechaliq,
                motivoLiquidacion = motivo,

                salarioPromedio = salProm,
                aniosAntiguedad = aniosTrab,
                diasPreaviso = diasPreav,
                fechaPreaviso = fechaPrev,
                diasVacacionesPendientes = diasVac, // Nota, estoy hay que hablarlo con el grupo

                pagoPreaviso = preaviso,
                pagoAguinaldoProp = aguinaldoProp,
                pagoVacacionesNG = vacFaltantes,
                pagoCesantia = cesantia,
                remuPendientes = pagosFaltantes, // preguntar como se hizo
                costoLiquidacion = costoLiq,

                observacionLiquidacion = observs,
                idEstado = 2
            };

            return liquid;
        }


        //  CALCULOS ------------------------------------------------------------------------------------------------------------------------------------------------

        // 1. Total MESES de trabajo en la empresa -------------------------------------------------------
        private int mesesDeTrabajo(DateTime entrada, DateTime salida)
        {
            DateTime ahora = salida;
            int meses = 0;

            // Si fue contratado este año
            if (ahora.Year == entrada.Year)
            {
                meses = ahora.Month - entrada.Month;
            }

            // Si fue contratado antes
            else
            {
                int aniosEnMes = (ahora.Year - entrada.Year) * 12;
                int difMeses = entrada.Month - ahora.Month;
                meses = aniosEnMes - difMeses;
            }
            return meses;
        }


        // 2. Cálculo del SALARIO PROMEDIO --------------------------------------------------------------
        private decimal calcularSalarioProm(int id, decimal salarioHoraExtra, int meses, DateTime fechaLiq)
        {
            // Valores Globales
            decimal salarioProm = 0;

            // Definición de tiempo de trabajo a calcular
            if (meses > 6) { meses = 6; }
            DateTime tiempo = fechaLiq.AddMonths(-meses); // Tiempo límite de 6 meses

            // Búsqueda de valores en remuneraciones de un empleado particular
            List<RemuneracionDto> remuneraciones = _remuneraciones.Listar(id);

            if (remuneraciones != null)
            {
                // Pasa por todos los valores
                for (int i = remuneraciones.Count - 1; i >= 0; i--)
                {
                    // Compara la fecha de la remuneración la fecha límite, 
                    int fecha = DateTime.Compare(remuneraciones[i].fechaRemuneracion, tiempo);

                    // Si la fecha es mayor, se adicionan valores 
                    if (fecha >= 0)
                    {
                        // Se adicionan los valores de la remuneracion
                        if (remuneraciones[i].pagoQuincenal != null) { salarioProm += Convert.ToDecimal(remuneraciones[i].pagoQuincenal); }
                        if (remuneraciones[i].comision != null) { salarioProm += Convert.ToDecimal(remuneraciones[i].comision); }
                    }
                    else { break; } // Se acaba el ciclo
                }
            }

            // Obtener el promedio
            salarioProm = salarioProm / meses;
            return salarioProm;
        }


        // 3. Cálculo de N° DIAS DE PREAVISO ------------------------------------------------------------
        private int diasPreaviso(int meses)
        {
            int dias = 0;

            if (meses < 3) { dias = 0; }
            else if (meses >= 3 && meses < 6) { dias = 7; }
            else if (meses >= 6 && meses < 12) { dias = 14; }
            else { dias = 30; }

            return dias;
        }


        // 4. Cálculo de FECHA DE PREAVISO ------------------------------------------------------------
        private string fechaDePreaviso(DateTime fechaliq, int diasPreav)
        {
            if (diasPreav == 0) { return "No aplica"; }
            return fechaliq.AddDays(-diasPreav).ToString();
        }


        // 5. Cálculo de PREAVISO -----------------------------------------------------------------------
        private decimal calculoPreaviso(string motivo, decimal salProm)
        {
            if (motivo.Equals("Pensión o muerte")) { return 0; }
            else { return salProm; }
        }


        // 6. Cálculo de AGUINALDO PROPORCIONAL ---------------------------------------------------------
        private decimal calculoAguinaldoP(int meses, DateTime salida, decimal salProm)
        {
            if (salida.Month < meses) { meses = salida.Month + 1; }
            decimal aguinaldo = salProm * meses;
            return aguinaldo;
        }


        // 7. Cálculo de cesantía ----------------------------------------------------------------------
        private decimal pagoCesantia(string motivo, int meses, decimal salDiario, int anios)
        {
            decimal cesantia = 0;

            // Si tiene alguno de estos motivos, se corta el proceso
            if (motivo.Equals("Despido Justificado") || motivo.Equals("Renuncia"))
            { return cesantia; }
            else
            {
                if (anios == 0)
                {
                    if (meses >= 3 && meses < 6) { cesantia = salDiario * 7; }           // si tiene entre 3 a 6 meses
                    else if (meses >= 6 && meses < 12) { cesantia = salDiario * 14; }    // si tiene entre 6 a 12 meses
                }
                else                                                                     // Si lleva más de un año
                {
                    // se calcula valor según años de trabajo                                    
                    switch (anios)
                    {
                        case 1:
                            cesantia = Decimal.Multiply(salDiario, 19.5m); // m para evitar ser leído como double
                            break;
                        case int n when (n == 3 || n == 12):
                            cesantia = (Decimal.Multiply(salDiario, 20.5m)) * n;
                            break;
                        case int n when (n == 4 || n == 11):
                            cesantia = (salDiario * 21) * n;
                            break;
                        case 5:
                            cesantia = (Decimal.Multiply(salDiario, 21.24m)) * 5;
                            break;
                        case int n when (n == 6 || n == 10):
                            cesantia = (Decimal.Multiply(salDiario, 21.24m)) * n;
                            break;
                        case int n when (n == 7 || n == 8 || n == 9):
                            cesantia = (salDiario * 22) * n;
                            break;
                        default:
                            cesantia = cesantia = (salDiario * 20) * anios;
                            break;
                    }
                }

                return cesantia;
            }
        }


        // 8. Cálculo días de vacaciones ----------------------------------------------------------------
        private int diasVacaciones(EmpleadoDto emp)
        {
            // Dias de vacaciones
            int dias = 14;
            decimal valorDiv = emp.salarioPoHora * 8;

            // Definición de tiempo de trabajo a calcular
            DateTime tiempo = new DateTime(DateTime.Now.Year, 1, 1).Date; //01/01/20XX

            // Búsqueda de valores en remuneraciones de un empleado particular
            List<RemuneracionDto> remuneraciones = _remuneraciones.Listar(emp.idEmpleado);

            if (remuneraciones != null)
            {
                // Va en retroceso, desde la última entrada a la primera
                for (int i = 0; i < remuneraciones.Count; i++)
                {
                    // Si la fecha es menor, se termina el ciclo
                    if (remuneraciones[i].fechaRemuneracion < tiempo) { break; }

                    // Aquí evaluación de si son vacaciones
                    if (remuneraciones[i].nombreTipoRemuneracion.Equals("Vacaciones"))
                    {
                        dias -= Convert.ToInt32(remuneraciones[i].pagoQuincenal / valorDiv);
                    }
                }
            }

            // Dias de vacaciones restantes
            return dias;

        }

        private decimal vacasFaltantes(decimal diasPreav ,  decimal salarioDiario)
        {
            decimal vf = diasPreav * salarioDiario;
            return vf;
        }


        // 9. Cálculo remuneraciones pendientes ---------------------------------------------------------
        private decimal remuneracionesPendientes(int id, DateTime fechaSalida, decimal salDiario)
        {
            decimal remuPen = 0;

            // Tomar el día de hoy
            DateTime today = DateTime.Today;

            // Checkpoints
            DateTime checkpoint1 = new DateTime(today.Year, today.Month, 15);
            DateTime checkpoint2 = checkpoint1.AddMonths(1).AddDays(-14);

            // Se mide cuantos días hay de diferencia
            int hoy = today.DayOfYear;
            int salida = fechaSalida.DayOfYear;
            int diferencia = salida - hoy;

            // Si la diferencia es positiva y si sobrepasa la quincena
            if (diferencia > 0) {
                int toCP = 0;           // to checkpoint
                int realDif = 0;        // diferencia real 

                // Verificar donde estamos con respecto al chekpoint
                if (today < checkpoint1) {
                    toCP = checkpoint1.Day - today.Day;     // cuanto falta para CP
                    realDif = diferencia - toCP;            // se le resta a la diferencia de días original
                }
                else {
                    toCP = checkpoint2.Day - today.Day;     // cuanto falta para CP
                    realDif = diferencia - toCP;            // se le resta a la diferencia de días original
                }

                if (realDif > 0)
                {
                    remuPen = realDif * salDiario;              // Se calcula con la diferencia real 
                }
            }

            return remuPen;
        }

    }
}
