using System;
using System.Collections.Generic;
using Emplaniapp.Abstracciones.InterfacesAD.Remuneraciones.CrearRemuneracion;
using Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones.CrearRemuneraciones;
using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Remuneraciones;

namespace Emplaniapp.LogicaDeNegocio.Remuneraciones.CrearRemuneraciones
{
    public class CrearRemuneracionesLN : ICrearRemuneracionesLN
    {
        private ICrearRemuneracionesAD _crearRemuneracionesAD;

        public CrearRemuneracionesLN()
        {
            _crearRemuneracionesAD = new CrearRemuneracionesAD();
        }

        public List<RemuneracionDto> GenerarRemuneracionesQuincenales(DateTime? fechaProceso = null)
        {
            try
            {
                return _crearRemuneracionesAD.GenerarRemuneracionesQuincenales(fechaProceso);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar remuneraciones quincenales: " + ex.Message);
            }
        }

        public void AgregarRemuneracionManual(RemuneracionDto remuneracionDto, int idEmpleado)
        {
            try
            {
                _crearRemuneracionesAD.AgregarRemuneracionManual(remuneracionDto, idEmpleado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar remuneración manual: " + ex.Message);
            }
        }
    }
}
