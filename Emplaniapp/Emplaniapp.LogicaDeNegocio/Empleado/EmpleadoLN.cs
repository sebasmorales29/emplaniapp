using Emplaniapp.Abstracciones.InterfacesParaUI.Empleado;
using Emplaniapp.Abstracciones.InterfacesAD.Empleado;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.AccesoADatos;
using Emplaniapp.AccesoADatos.Empleado;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Emplaniapp.LogicaDeNegocio.Empleado
{
    public class EmpleadoLN : IEmpleadoLN
    {
        private Contexto contexto;
        private IEmpleadoAD empleadoAD;

        public EmpleadoLN()
        {
            contexto = new Contexto();
            empleadoAD = new EmpleadoAD();
        }

        public List<EmpleadoDto> ListarTodos()
        {
            try
            {
                var empleados = from e in contexto.Empleados
                               join c in contexto.Cargos on e.idCargo equals c.Id into cargoJoin
                               from cargo in cargoJoin.DefaultIfEmpty()
                               join m in contexto.TiposMoneda on e.idTipoMoneda equals m.Id into monedaJoin
                               from moneda in monedaJoin.DefaultIfEmpty()
                               join b in contexto.Bancos on e.idBanco equals b.Id into bancoJoin
                               from banco in bancoJoin.DefaultIfEmpty()
                               join est in contexto.Estados on e.idEstado equals est.Id into estadoJoin
                               from estado in estadoJoin.DefaultIfEmpty()
                               where e.idEstado != 3 // Excluir eliminados
                               select new EmpleadoDto
                               {
                                   idEmpleado = e.Id,
                                   nombre = e.nombre,
                                   segundoNombre = e.segundoNombre,
                                   primerApellido = e.primerApellido,
                                   segundoApellido = e.segundoApellido,
                                   fechaNacimiento = e.fechaNacimiento,
                                   cedula = e.cedula,
                                   numeroTelefonico = e.numeroTelefonico,
                                   correoInstitucional = e.correoInstitucional,
                                   idDireccion = e.idDireccion,
                                   idCargo = e.idCargo,
                                   nombreCargo = cargo != null ? cargo.nombreCargo : "",
                                   fechaContratacion = e.fechaContratacion,
                                   fechaSalida = e.fechaSalida,
                                   periocidadPago = e.periocidadPago,
                                   salarioDiario = (double)e.salarioDiario,
                                   salarioAprobado = (double)e.salarioAprobado,
                                   salarioPorMinuto = (double)e.salarioPorMinuto,
                                   salarioPoHora = (double)e.salarioPoHora,
                                   salarioPorHoraExtra = (double)e.salarioPorHoraExtra,
                                   idMoneda = e.idTipoMoneda,
                                   nombreMoneda = moneda != null ? moneda.nombreMoneda : "",
                                   cuentaIBAN = e.cuentaIBAN,
                                   idBanco = e.idBanco,
                                   nombreBanco = banco != null ? banco.nombreBanco : "",
                                   idEstado = e.idEstado,
                                   nombreEstado = estado != null ? estado.nombreEstado : ""
                               };

                return empleados.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar empleados: " + ex.Message);
            }
        }

        public EmpleadoDto ObtenerPorId(int id)
        {
            try
            {
                var empleado = (from e in contexto.Empleados
                               join c in contexto.Cargos on e.idCargo equals c.Id into cargoJoin
                               from cargo in cargoJoin.DefaultIfEmpty()
                               join m in contexto.TiposMoneda on e.idTipoMoneda equals m.Id into monedaJoin
                               from moneda in monedaJoin.DefaultIfEmpty()
                               join b in contexto.Bancos on e.idBanco equals b.Id into bancoJoin
                               from banco in bancoJoin.DefaultIfEmpty()
                               join est in contexto.Estados on e.idEstado equals est.Id into estadoJoin
                               from estado in estadoJoin.DefaultIfEmpty()
                               where e.Id == id && e.idEstado != 3
                               select new EmpleadoDto
                               {
                                   idEmpleado = e.Id,
                                   nombre = e.nombre,
                                   segundoNombre = e.segundoNombre,
                                   primerApellido = e.primerApellido,
                                   segundoApellido = e.segundoApellido,
                                   fechaNacimiento = e.fechaNacimiento,
                                   cedula = e.cedula,
                                   numeroTelefonico = e.numeroTelefonico,
                                   correoInstitucional = e.correoInstitucional,
                                   idDireccion = e.idDireccion,
                                   idCargo = e.idCargo,
                                   nombreCargo = cargo != null ? cargo.nombreCargo : "",
                                   fechaContratacion = e.fechaContratacion,
                                   fechaSalida = e.fechaSalida,
                                   periocidadPago = e.periocidadPago,
                                   salarioDiario = (double)e.salarioDiario,
                                   salarioAprobado = (double)e.salarioAprobado,
                                   salarioPorMinuto = (double)e.salarioPorMinuto,
                                   salarioPoHora = (double)e.salarioPoHora,
                                   salarioPorHoraExtra = (double)e.salarioPorHoraExtra,
                                   idMoneda = e.idTipoMoneda,
                                   nombreMoneda = moneda != null ? moneda.nombreMoneda : "",
                                   cuentaIBAN = e.cuentaIBAN,
                                   idBanco = e.idBanco,
                                   nombreBanco = banco != null ? banco.nombreBanco : "",
                                   idEstado = e.idEstado,
                                   nombreEstado = estado != null ? estado.nombreEstado : ""
                               }).FirstOrDefault();

                return empleado;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener empleado: " + ex.Message);
            }
        }

        public bool Insertar(EmpleadoDto empleadoDto)
        {
            try
            {
                var empleado = new Abstracciones.ModelosAD.Empleado
                {
                    nombre = empleadoDto.nombre,
                    segundoNombre = empleadoDto.segundoNombre,
                    primerApellido = empleadoDto.primerApellido,
                    segundoApellido = empleadoDto.segundoApellido,
                    fechaNacimiento = empleadoDto.fechaNacimiento,
                    cedula = empleadoDto.cedula,
                    numeroTelefonico = empleadoDto.numeroTelefonico,
                    correoInstitucional = empleadoDto.correoInstitucional,
                    idDireccion = empleadoDto.idDireccion,
                    idCargo = empleadoDto.idCargo,
                    fechaContratacion = empleadoDto.fechaContratacion,
                    fechaSalida = empleadoDto.fechaSalida,
                    periocidadPago = empleadoDto.periocidadPago,
                    salarioDiario = (decimal)empleadoDto.salarioDiario,
                    salarioAprobado = (decimal)empleadoDto.salarioAprobado,
                    salarioPorMinuto = (decimal)empleadoDto.salarioPorMinuto,
                    salarioPoHora = (decimal)empleadoDto.salarioPoHora,
                    salarioPorHoraExtra = (decimal)empleadoDto.salarioPorHoraExtra,
                    idTipoMoneda = empleadoDto.idMoneda,
                    cuentaIBAN = empleadoDto.cuentaIBAN,
                    idBanco = empleadoDto.idBanco,
                    idEstado = empleadoDto.idEstado
                };

                return empleadoAD.Insertar(empleado);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR EN INSERTAR LN: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"INNER EXCEPTION LN: {ex.InnerException.Message}");
                }
                throw new Exception("Error al insertar empleado: " + ex.Message);
            }
        }

        public bool Actualizar(EmpleadoDto empleadoDto)
        {
            try
            {
                var empleado = new Abstracciones.ModelosAD.Empleado
                {
                    Id = empleadoDto.idEmpleado,
                    nombre = empleadoDto.nombre,
                    segundoNombre = empleadoDto.segundoNombre,
                    primerApellido = empleadoDto.primerApellido,
                    segundoApellido = empleadoDto.segundoApellido,
                    fechaNacimiento = empleadoDto.fechaNacimiento,
                    cedula = empleadoDto.cedula,
                    numeroTelefonico = empleadoDto.numeroTelefonico,
                    correoInstitucional = empleadoDto.correoInstitucional,
                    idDireccion = empleadoDto.idDireccion,
                    idCargo = empleadoDto.idCargo,
                    fechaContratacion = empleadoDto.fechaContratacion,
                    fechaSalida = empleadoDto.fechaSalida,
                    periocidadPago = empleadoDto.periocidadPago,
                    salarioDiario = (decimal)empleadoDto.salarioDiario,
                    salarioAprobado = (decimal)empleadoDto.salarioAprobado,
                    salarioPorMinuto = (decimal)empleadoDto.salarioPorMinuto,
                    salarioPoHora = (decimal)empleadoDto.salarioPoHora,
                    salarioPorHoraExtra = (decimal)empleadoDto.salarioPorHoraExtra,
                    idTipoMoneda = empleadoDto.idMoneda,
                    cuentaIBAN = empleadoDto.cuentaIBAN,
                    idBanco = empleadoDto.idBanco,
                    idEstado = empleadoDto.idEstado
                };

                return empleadoAD.Actualizar(empleado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar empleado: " + ex.Message);
            }
        }

        public bool Eliminar(int id)
        {
            try
            {
                return empleadoAD.Eliminar(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar empleado: " + ex.Message);
            }
        }

        public bool ActivarDesactivar(int id, int nuevoEstado)
        {
            try
            {
                return empleadoAD.ActivarDesactivar(id, nuevoEstado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al activar/desactivar empleado: " + ex.Message);
            }
        }

        public List<CargoDto> ObtenerCargos()
        {
            try
            {
                var cargos = (from c in contexto.Cargos
                             select new CargoDto
                             {
                                 idCargo = c.Id,
                                 nombreCargo = c.nombreCargo,
                                 idNumeroOcupacion = c.idNumeroOcupacion
                             }).ToList();
                return cargos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener cargos: " + ex.Message);
            }
        }

        public List<MonedaDto> ObtenerTiposMoneda()
        {
            try
            {
                var monedas = (from m in contexto.TiposMoneda
                              select new MonedaDto
                              {
                                  idMoneda = m.Id,
                                  nombreMoneda = m.nombreMoneda
                              }).ToList();
                return monedas;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener tipos de moneda: " + ex.Message);
            }
        }

        public List<BancoDto> ObtenerBancos()
        {
            try
            {
                var bancos = (from b in contexto.Bancos
                             select new BancoDto
                             {
                                 idBanco = b.Id,
                                 nombreBanco = b.nombreBanco
                             }).ToList();
                return bancos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener bancos: " + ex.Message);
            }
        }

        public List<EstadoDto> ObtenerEstados()
        {
            try
            {
                var estados = (from e in contexto.Estados
                              select new EstadoDto
                              {
                                  idEstado = e.Id,
                                  nombreEstado = e.nombreEstado
                              }).ToList();
                return estados;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener estados: " + ex.Message);
            }
        }
    }
} 