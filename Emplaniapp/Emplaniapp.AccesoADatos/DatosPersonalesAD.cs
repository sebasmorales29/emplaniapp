using Emplaniapp.Abstracciones.InterfacesAD;
using Emplaniapp.Abstracciones.ModelosParaUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.AccesoADatos
{
    public class DatosPersonalesAD : IDatosPersonalesAD
    {
        public EmpleadoDto ObtenerEmpleadoPorId(int idEmpleado)
        {
            using (var contexto = new Contexto())
            {
                var empleado = contexto.Empleado.FirstOrDefault(e => e.idEmpleado == idEmpleado);
                if (empleado == null) return null;

                // Obtener información relacionada
                var cargo = contexto.Cargos.FirstOrDefault(c => c.idCargo == empleado.idCargo);
                var estado = contexto.Estado.FirstOrDefault(e => e.idEstado == empleado.idEstado);

                return new EmpleadoDto
                {
                    idEmpleado = empleado.idEmpleado,
                    nombre = empleado.nombre,
                    segundoNombre = empleado.segundoNombre,
                    primerApellido = empleado.primerApellido,
                    segundoApellido = empleado.segundoApellido,
                    fechaNacimiento = empleado.fechaNacimiento,
                    cedula = empleado.cedula,
                    numeroTelefonico = empleado.numeroTelefonico,
                    correoInstitucional = empleado.correoInstitucional,
                    idDireccion = empleado.idDireccion,
                    idCargo = empleado.idCargo,
                    nombreCargo = cargo?.nombreCargo ?? "Sin cargo",
                    fechaContratacion = empleado.fechaContratacion,
                    fechaSalida = empleado.fechaSalida,
                    periocidadPago = empleado.periocidadPago,
                    salarioDiario = empleado.salarioDiario,
                    salarioAprobado = empleado.salarioAprobado,
                    salarioPorMinuto = empleado.salarioPorMinuto,
                    salarioPoHora = empleado.salarioPoHora,
                    salarioPorHoraExtra = empleado.salarioPorHoraExtra,
                    idMoneda = empleado.idTipoMoneda,
                    nombreMoneda = "Colones", // Por ahora hardcodeado, después implementar tabla TipoMoneda
                    cuentaIBAN = empleado.cuentaIBAN,
                    idBanco = empleado.idBanco,
                    nombreBanco = "BN", // Por ahora hardcodeado, después implementar tabla Bancos
                    idEstado = empleado.idEstado,
                    nombreEstado = estado?.nombreEstado ?? "Sin estado"
                };
            }
        }

        public bool ActualizarDatosPersonales(EmpleadoDto empleadoDto)
        {
            try
            {
                using (var contexto = new Contexto())
                {
                    var empleado = contexto.Empleado.FirstOrDefault(e => e.idEmpleado == empleadoDto.idEmpleado);
                    if (empleado == null) return false;

                    empleado.nombre = empleadoDto.nombre;
                    empleado.segundoNombre = empleadoDto.segundoNombre;
                    empleado.primerApellido = empleadoDto.primerApellido;
                    empleado.segundoApellido = empleadoDto.segundoApellido;
                    empleado.fechaNacimiento = empleadoDto.fechaNacimiento;
                    empleado.cedula = empleadoDto.cedula;
                    empleado.numeroTelefonico = empleadoDto.numeroTelefonico;
                    empleado.correoInstitucional = empleadoDto.correoInstitucional;

                    contexto.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool ActualizarDatosLaborales(int idEmpleado, int idCargo, DateTime fechaContratacion, DateTime? fechaSalida)
        {
            try
            {
                using (var contexto = new Contexto())
                {
                    var empleado = contexto.Empleado.FirstOrDefault(e => e.idEmpleado == idEmpleado);
                    if (empleado == null) return false;

                    empleado.idCargo = idCargo;
                    empleado.fechaContratacion = fechaContratacion;
                    empleado.fechaSalida = fechaSalida;

                    contexto.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool ActualizarDatosFinancieros(int idEmpleado, decimal salarioAprobado, decimal salarioDiario, 
            string periocidadPago, int idTipoMoneda, string cuentaIBAN, int idBanco)
        {
            try
            {
                using (var contexto = new Contexto())
                {
                    var empleado = contexto.Empleado.FirstOrDefault(e => e.idEmpleado == idEmpleado);
                    if (empleado == null) return false;

                    empleado.salarioAprobado = salarioAprobado;
                    empleado.salarioDiario = salarioDiario;
                    empleado.periocidadPago = periocidadPago;
                    empleado.idTipoMoneda = idTipoMoneda;
                    empleado.cuentaIBAN = cuentaIBAN;
                    empleado.idBanco = idBanco;

                    // Recalcular salarios basados en el salario aprobado
                    if (periocidadPago == "Quincenal")
                    {
                        empleado.salarioDiario = salarioAprobado / 15;
                    }
                    else if (periocidadPago == "Mensual")
                    {
                        empleado.salarioDiario = salarioAprobado / 30;
                    }

                    empleado.salarioPoHora = empleado.salarioDiario / 8;
                    empleado.salarioPorMinuto = empleado.salarioPoHora / 60;
                    empleado.salarioPorHoraExtra = empleado.salarioPoHora * 1.5m;

                    contexto.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public List<CargoDto> ObtenerCargos()
        {
            using (var contexto = new Contexto())
            {
                return contexto.Cargos.Select(c => new CargoDto
                {
                    idCargo = c.idCargo,
                    nombreCargo = c.nombreCargo
                }).ToList();
            }
        }

        public List<MonedaDto> ObtenerTiposMoneda()
        {
            // Por ahora retornamos datos hardcodeados
            return new List<MonedaDto>
            {
                new MonedaDto { idMoneda = 1, nombreMoneda = "Colones" },
                new MonedaDto { idMoneda = 2, nombreMoneda = "Dólares" }
            };
        }

        public List<BancoDto> ObtenerBancos()
        {
            // Por ahora retornamos datos hardcodeados
            return new List<BancoDto>
            {
                new BancoDto { idBanco = 1, nombreBanco = "Banco Nacional" },
                new BancoDto { idBanco = 2, nombreBanco = "Banco de Costa Rica" },
                new BancoDto { idBanco = 3, nombreBanco = "BAC Credomatic" },
                new BancoDto { idBanco = 4, nombreBanco = "Banco Popular" }
            };
        }
    }
} 