﻿using Emplaniapp.Abstracciones.ModelosParaUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.InterfacesParaUI.Tipo_Remuneracion
{
    public interface IAgregarTipoRemuneracionLN
    {
        Task<int> Guardar(TipoRemuneracionDto tipoRemu);
    }
}
