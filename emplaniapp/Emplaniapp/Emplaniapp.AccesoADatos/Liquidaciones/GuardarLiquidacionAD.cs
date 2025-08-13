﻿using Emplaniapp.Abstracciones.InterfacesAD.Liquidaciones;
using Emplaniapp.Abstracciones.ModelosAD;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.AccesoADatos.Liquidaciones
{
    public class GuardarLiquidacionAD : IGuardarLiquidacionAD
    {

        Contexto contexto;

        public GuardarLiquidacionAD()
        {
            contexto = new Contexto();
        }


        public async Task<int> Guardar(Liquidacion Liquid)
        {
            Liquid.idEstado = 1; // Se establece el estado como "Activo" por defecto
            contexto.Liquidaciones.Add(Liquid);
            EntityState estado = contexto.Entry(Liquid).State = System.Data.Entity.EntityState.Added;
            int liquidGuardado = await contexto.SaveChangesAsync();
            return liquidGuardado;
        }
    }
}
