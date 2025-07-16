using System;
using System.Collections.Generic;
using Emplaniapp.Abstracciones.InterfacesAD.Remuneraciones.CrearRemuneracion;

using Emplaniapp.Abstracciones.ModelosParaUI;
using Emplaniapp.AccesoADatos.Remuneraciones;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesParaUI.Remuneraciones.CrearRemuneraciones;

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

        public async Task<int> AgregarRemuneracionManual(RemuneracionDto remuneracionDto){
            remuneracionDto.fechaRemuneracion = DateTime.Now;
            remuneracionDto.idEstado = 1;
            int cantidadDeResultados = await _crearRemuneracionesAD.AgregarRemuneracionManual(remuneracionDto);
            return cantidadDeResultados;
        }
    }
}
