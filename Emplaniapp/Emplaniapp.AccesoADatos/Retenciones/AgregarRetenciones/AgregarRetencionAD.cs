using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Emplaniapp.Abstracciones.InterfacesAD.Remuneraciones.CrearRemuneracion;
using Emplaniapp.Abstracciones.InterfacesAD.Retenciones;
using Emplaniapp.Abstracciones.ModelosAD;
using Emplaniapp.Abstracciones.ModelosParaUI;


namespace Emplaniapp.AccesoADatos.Retenciones
{
    public class AgregarRetencionAD : IAgregarRetencionAD
    {
        private readonly Contexto _ctx = new Contexto();
        public void Agregar(Retencion retencion)
        {
            _ctx.Retenciones.Add(retencion);
            _ctx.SaveChanges();
        }
    }
}