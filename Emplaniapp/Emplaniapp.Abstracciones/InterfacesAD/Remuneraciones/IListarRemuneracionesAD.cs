﻿using Emplaniapp.Abstracciones.ModelosParaUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.Abstracciones.InterfacesAD.Remuneraciones
{
    public interface IListarRemuneracionesAD
    {
        List<RemuneracionDto> Listar(int id);
    }
}
